using Exchange.Context.Context;
using Exchange.Services.ValutaRate.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Exchange.ExchangeVolute.Controllers;
[ApiController]
[Route("api")]
public class ValuteController(IVoluteRateService voluteRateService) : ControllerBase
{
    private readonly IVoluteRateService _voluteRateService = voluteRateService;
    [HttpGet("/daily")]
    public async Task<IActionResult> GetCurrentValue(string date)
    {
        if (DateOnly.TryParseExact(date,"dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly parsedDate))
        {
            return Ok(await _voluteRateService.GetCursByDateAsync(parsedDate));
        }
        else
        {
            return BadRequest("Неправильный тип данных. Используйте 'dd.MM.yyyy'.");
        }
    }

    [HttpGet("/dynamic")]
    public async Task<IActionResult> GetDynamicValue(string date1, string date2, string name)
    {
        if (DateOnly.TryParseExact(date1, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly parsedDate1) &&
                        DateOnly.TryParseExact(date2, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly parsedDate2))
        {
            var result = await _voluteRateService.GetCursListByDateAsync(parsedDate1, parsedDate2, name);
            return Ok(result);
        }
        else
        {
            return BadRequest("Неправильный тип данных. Используйте 'dd.MM.yyyy'.");
        }
    }
}
