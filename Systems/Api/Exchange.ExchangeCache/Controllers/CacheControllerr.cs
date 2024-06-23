using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace Exchange.ExchangeCache.Controllers
{
    [ApiController]
    [Route("api")]
    public class CacheControllerr : ControllerBase
    {
        [HttpGet("/daily")]
        public async Task<IActionResult> getData(string date)
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:7192");
            var client = new ExchangeCacheService.Volute.VoluteClient(channel);

            var reply = await client.GetCurrentValueAsync(new ExchangeCacheService.DailyValuteRequest() { Date = date });
            return Ok(reply);
        }

        [HttpGet("/dynamic")]
        public async Task<IActionResult> GetDynamicValue(string date1, string date2, string name)
        {
            if (DateOnly.TryParseExact(date1, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly parsedDate1) &&
                            DateOnly.TryParseExact(date2, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly parsedDate2))
            {
                using var channel = GrpcChannel.ForAddress("https://localhost:7192");
                var client = new ExchangeCacheService.Volute.VoluteClient(channel);
                var result = await client.GetDynamicValueAsync(new ExchangeCacheService.DynamicValueRequest()
                {
                    Date1 = date1,
                    Date2 = date2,
                    Name = name
                });
                return Ok(result);
            }
            else
            {
                return BadRequest("Неправильный тип данных. Используйте 'dd.MM.yyyy'.");
            }
        }
    }

}
