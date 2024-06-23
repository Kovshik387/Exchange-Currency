using Exchange.Services.ValutaRate.Data.DTO;

namespace Exchange.Services.ValutaRate.Infrastructure;
/// <summary>
/// Интерфейс для сервиса получения и выдачи курсов валют
/// </summary>
public interface IVoluteRateService
{
    /// <summary>
    /// Полученеие котировок за указанный день
    /// </summary>
    /// <param name="date">В случае, если аргумент равен null, будут получены данные за сегодняшнюю дату.</param>
    /// <returns></returns>
    public Task<RateValueDTO?> GetCursByDateAsync(DateOnly? date = null);
    /// <summary>
    /// Получение котировок за указанный интервал
    /// </summary>
    /// <param name="date1">Первая дата</param>
    /// <param name="date2">Вторая дата</param>
    /// <param name="nameVal">Имя валюты</param>
    /// <returns></returns>
    public Task<IList<RecordDTO>> GetCursListByDateAsync(DateOnly date1, DateOnly date2, string nameVal);
}
