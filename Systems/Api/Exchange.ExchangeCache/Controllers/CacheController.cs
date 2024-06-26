using Exchange.Services.Cache.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Exchange.ExchangeCache.Controllers
{
    [ApiController]
    [Route("api")]
    public class CacheController(ICacheService cacheService) : ControllerBase
    {
        private readonly ICacheService _cacheService = cacheService;
        
        [HttpGet("/daily")]
        public async Task<IActionResult> GetData(string date)
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

        [HttpGet("/dynamic")]
        public async Task<IActionResult> GetDynamicValue(string date1, string date2, string name)
        {
            if (DateOnly.TryParseExact(date1, "dd.MM.yyyy", out _) && DateOnly.TryParseExact(date2, "dd.MM.yyyy", out _))
            {
                return Ok(await _cacheService.GetDataByDatesAsync(date1, date2,name));
            }
            else
            {
                return BadRequest("Invalid data type. Use 'dd.MM.yyyy'.");
            }
        }
    }

}
