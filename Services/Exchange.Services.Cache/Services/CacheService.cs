using Exchange.Services.Cache.Data.DTO;
using Exchange.Services.Cache.Infrastructure;
using Exchange.Services.Settings.SettingsConfigure;
using Microsoft.Extensions.Caching.Distributed;
using Grpc.Net.Client;
using Microsoft.Extensions.Logging;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Exchange.Services.Cache.Services;

public class CacheService(IDistributedCache distributedCache,
    ApiEndPointSettings pointSettings, ILogger<CacheService> logger) : ICacheService
{
    private readonly IDistributedCache _cache = distributedCache;
    private readonly ApiEndPointSettings _pointSettings = pointSettings;
    private readonly ILogger<CacheService> _logger = logger;
    public async Task<ExchangeCacheService.DailyValuteResponse?> GetDataByDateAsync(string date)
    {
        ExchangeCacheService.DailyValuteResponse? result = null;

        string? requestString = await _cache.GetStringAsync(date);

        if (requestString is not null)
            result = JsonConvert.DeserializeObject<ExchangeCacheService.DailyValuteResponse>(requestString, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateFormatString = "dd.MM.yyyy"
            });

        if (result is null)
        {
            _logger.LogInformation(_pointSettings.GrpcServerPath);
            using var channel = GrpcChannel.ForAddress(_pointSettings.GrpcServerPath);
            var client = new ExchangeCacheService.Volute.VoluteClient(channel);
            result = await client.GetCurrentValueAsync(new ExchangeCacheService.DailyValuteRequest() { Date = date });

            var serializedResult = JsonConvert.SerializeObject(result, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateFormatString = "dd.MM.yyyy"
            });

            await _cache.SetStringAsync(date, serializedResult, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(30)
            });

            _logger.LogInformation("Data from database");
        }
        else _logger.LogInformation("Data from cache");

        return result;
    }

    public async Task<ExchangeCacheService.DynamicValueResponse?> GetDataByDatesAsync(string date1, string date2, string name)
    {
        ExchangeCacheService.DynamicValueResponse? result = null;
        string searchSting = $"{date1} {date2} {name}";
        string? requestString = await _cache.GetStringAsync(searchSting);

        if (requestString is not null)
            result = JsonConvert.DeserializeObject<ExchangeCacheService.DynamicValueResponse>(requestString, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateFormatString = "dd.MM.yyyy",
            });

        if (result is null)
        {
            _logger.LogInformation(_pointSettings.GrpcServerPath);
            using var channel = GrpcChannel.ForAddress(_pointSettings.GrpcServerPath);
            var client = new ExchangeCacheService.Volute.VoluteClient(channel);
            result = await client.GetDynamicValueAsync(new ExchangeCacheService.DynamicValueRequest() { Date1 = date1, Date2 = date2, Name = name });

            var serializedResult = JsonConvert.SerializeObject(result, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateFormatString = "dd.MM.yyyy"
            });

            await _cache.SetStringAsync(searchSting, serializedResult, new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(20)
            });

            _logger.LogInformation("Data from database");
        }
        else _logger.LogInformation("Data from cache");

        return result;
    }
}
