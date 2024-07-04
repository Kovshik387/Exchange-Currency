using ExchangeServiceProto;

namespace Exchange.Services.Cache.Infrastructure;

/// <summary>
/// Интерфейс кеширования данных
/// </summary>
public interface ICacheService
{
    public Task<DailyValuteResponse?> GetDataByDateAsync(string date);

    public Task<DynamicValueResponse?> GetDataByDatesAsync(string date1, string date2, string name);
}
