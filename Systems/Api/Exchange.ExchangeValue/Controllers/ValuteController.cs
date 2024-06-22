using Exchange.ExchangeVolute.Data;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Xml.Serialization;

namespace Exchange.ExchangeVolute.Controllers;
[ApiController]
public class ValuteController : ControllerBase
{
    [HttpGet("/get")]
    public async Task<IActionResult> GetCurrentValue()
    {
        string url = "http://www.cbr.ru/scripts/XML_daily.asp";
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        try
        {
            using HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            using Stream responseStream = await response.Content.ReadAsStreamAsync();
            XmlSerializer serializer = new XmlSerializer(typeof(ValCurs));

            using (StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("windows-1251")))
            {
                ValCurs result = (ValCurs)serializer.Deserialize(reader)!;
                return Ok(result.Volute.First().Value);
            }

        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
