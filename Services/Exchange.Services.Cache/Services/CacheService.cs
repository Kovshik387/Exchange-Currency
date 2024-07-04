using Exchange.Services.Cache.Infrastructure;
using Exchange.Services.Settings.SettingsConfigure;
using Microsoft.Extensions.Caching.Distributed;
using Grpc.Net.Client;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ExchangeServiceProto;

namespace Exchange.Services.Cache.Services;

public class CacheService : ICacheService
{
    private readonly IDistributedCache _cache;
    private readonly RedisSettings _redisSettings;
    private readonly ApiEndPointSettings _endPointSettings;
    private readonly ILogger<CacheService> _logger;
    private readonly JsonSerializerSettings _serializerSettings;
    private readonly string _dateFormatString = "dd.MM.yyyy";

    public CacheService(IDistributedCache distributedCache, RedisSettings redisSettings, ApiEndPointSettings pointSettings, ILogger<CacheService> logger)
    {
        _cache = distributedCache;
        _redisSettings = redisSettings;
        _endPointSettings = pointSettings;
        _logger = logger;
        _serializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            DateFormatString = _dateFormatString
        };
    }
    public async Task<DailyValuteResponse?> GetDataByDateAsync(string date)
    {
        return await GetDataVolute<DailyValuteResponse>(date,
            async () => await GetDailyValuteFromSource(date),
            TimeSpan.FromMinutes(_redisSettings.CacheSmallData));
    }

    public async Task<DynamicValueResponse?> GetDataByDatesAsync(string date1, string date2, string name)
    {
        string searchString = $"{date1} {date2} {name}";
        return await GetDataVolute<DynamicValueResponse>(searchString,
            async () => await GetDynamicValueFromSource(date1, date2, name),
            TimeSpan.FromMinutes(_redisSettings.CacheLargeData));
    }

    private async Task<TData?> GetDataVolute<TData>(string cacheKey, Func<Task<TData>> getDataFromSource, TimeSpan cacheDuration)
    {
        TData? result = default;
        string? requestString = await _cache.GetStringAsync(cacheKey);

        if (requestString is not null)
        {
            result = JsonConvert.DeserializeObject<TData>(requestString, _serializerSettings);
        }

        if (result is null)
        {
            result = await getDataFromSource();

            var serializedResult = JsonConvert.SerializeObject(result, _serializerSettings);
            await _cache.SetStringAsync(cacheKey, serializedResult, new DistributedCacheEntryOptions
            {
                SlidingExpiration = cacheDuration
            });

            _logger.LogInformation("Data from database");
        }
        else
        {
            _logger.LogInformation("Data from cache");
        }

        return result;
    }

    private async Task<DailyValuteResponse> GetDailyValuteFromSource(string date)
    {
        using var channel = GrpcChannel.ForAddress(_endPointSettings.GrpcServerPath);
        var client = new Volute.VoluteClient(channel);
        return await client.GetCurrentValueAsync(new DailyValuteRequest { Date = date });
    }

    private async Task<DynamicValueResponse> GetDynamicValueFromSource(string date1, string date2, string name)
    {
        using var channel = GrpcChannel.ForAddress(_endPointSettings.GrpcServerPath);
        var client = new Volute.VoluteClient(channel);
        return await client.GetDynamicValueAsync(new DynamicValueRequest { Date1 = date1, Date2 = date2, Name = name });
    }
}
