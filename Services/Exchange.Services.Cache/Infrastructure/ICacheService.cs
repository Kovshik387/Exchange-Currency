using Exchange.Services.Cache.Data.DTO;

namespace Exchange.Services.Cache.Infrastructure;

/// <summary>
/// Интерфейс кеширования данных
/// </summary>
public interface ICacheService
{
    public Task<ExchangeCacheService.DailyValuteResponse?> GetDataByDateAsync(string date);

    public Task<ExchangeCacheService.DynamicValueResponse?> GetDataByDatesAsync(string date1, string date2, string name);
}
