using Exchange.Services.Cache.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Exchange.ExchangeCache.Controllers
{
    [ApiController]
    [Route("api")]
    public class CacheController : ControllerBase
    {
        private readonly ICacheService _cacheService;
        public CacheController(ICacheService cacheService) => _cacheService = cacheService;
        
        [HttpGet("/exchange-rates")]
        public async Task<IActionResult> GetData([FromQuery] string date)
        {
            if (DateOnly.TryParseExact(date, "dd.MM.yyyy",out _))
            {
                return Ok(await _cacheService.GetDataByDateAsync(date));
            }
            else
            {
                return BadRequest("Invalid data type. Use 'dd.MM.yyyy'.");
            }
        }

        [HttpGet("/exchange-rate")]
        public async Task<IActionResult> GetDynamicValue([FromQuery] string date1, [FromQuery] string date2, [FromQuery] string name)
        {
            if (DateOnly.TryParseExact(date1, "dd.MM.yyyy", out _) && DateOnly.TryParseExact(date2, "dd.MM.yyyy", out _))
            {
                return Ok(await _cacheService.GetDataByDatesAsync(date1, date2, name));
            }
            else
            {
                return BadRequest("Invalid data type. Use 'dd.MM.yyyy'.");
            }
        }
    }

}
