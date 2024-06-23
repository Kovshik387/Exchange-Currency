using AutoMapper;
using Castle.Core.Logging;
using Exchange.Common.Exceptions;
using Exchange.Context.Context;
using Exchange.Entities;
using Exchange.Services.ValutaRate.Data.DTO;
using Exchange.Services.ValutaRate.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Text;
using System.Xml.Serialization;

namespace Exchange.Services.ValutaRate.Services;
/// <summary>
/// Реализация интерфейса <see cref="IVoluteRateService"/> курсов валют
/// </summary>
/// <param name="context"></param>
/// <param name="mapper"></param>
/// <param name="logger"></param>
public class VoluteRateService(RateDbContext context, IMapper mapper, ILogger<VoluteRateService> logger) : IVoluteRateService
{
    private readonly string urlDaily = "http://www.cbr.ru/scripts/XML_daily.asp";
    private readonly string urlInterval = "http://www.cbr.ru/scripts/XML_dynamic.asp";
    private readonly RateDbContext _context = context;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<VoluteRateService> _logger = logger;

    public async Task<RateValueDTO?> GetCursByDateAsync(DateOnly? date = null)
    {
        var dataNow = DateOnly.FromDateTime(DateTime.UtcNow);
        
        if (date > dataNow) return null;

        DateOnly dateSearch = date ?? dataNow;

        if (DateTime.UtcNow.Hour > 15 && date.Equals(dataNow)) dateSearch = dateSearch.AddDays(-1);


        var data = await _context.RateValues.FirstOrDefaultAsync(x => x.Date.Equals(dateSearch));
        
        if (data is null)
        {
            var result = await GetValuteDataFromApiSingleAsync(
                date is null ? this.urlDaily : urlDaily + "?date_req=" + dateSearch.ToString()!.Replace(".","/")
            );

            if (result is null) return result;

            await _context.RateValues.AddAsync(_mapper.Map<RateValue>(result)); await _context.SaveChangesAsync();

            _logger.LogInformation("Запись данных в бд");
            return result;
        }
        else
        {
            _logger.LogInformation("Данные из бд");
            return _mapper.Map<RateValueDTO>(data);
        }

    }

    public async Task<IList<RecordDTO>> GetCursListByDateAsync(DateOnly date1, DateOnly date2, string nameVal)
    {
        var result = await GetValuteDataFromApiAsync($"{urlInterval}?date_req1={date1}&date_req2={date2}&VAL_NM_RQ={nameVal}");

        if (result == null) return null;

        var dataFill = await _context.Volutes.FirstOrDefaultAsync(x => x.Idname.Equals(nameVal));

        if (dataFill is null) return null;

        foreach (var record in result.Records)
        {
            var recordDate = DateOnly.Parse(record.Date);

            if (!_context.Volutes.Any(v => v.Idname.Equals(record.Id) && v.Valcurs.Date.Equals(recordDate)))
            {
                var rateValue = _context.RateValues.FirstOrDefault(rv => rv.Date == recordDate);

                if (rateValue == null)
                {
                    rateValue = new RateValue
                    {
                        Date = recordDate,
                        Name = result.Name
                    };
                    _context.RateValues.Add(rateValue);
                    await _context.SaveChangesAsync();
                }

                var volute = new Volute
                {
                    Idname = record.Id,
                    Nominal = record.Nominal,
                    Value = record.Value,
                    Vunitrate = record.VunitRate,
                    Valcurs = rateValue,
                    Charcode = dataFill!.Charcode,
                    Name = dataFill.Name,
                    Numcode = dataFill.Numcode,
                };
                _context.Volutes.Add(volute);
            }
        }
        await _context.SaveChangesAsync();
        return result.Records;
    }
        
    private async Task<RatesValueDTO?> GetValuteDataFromApiAsync(string url)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        using HttpClient client = new HttpClient();
        HttpResponseMessage response = await client.GetAsync(url);

        if (!response.IsSuccessStatusCode) throw 
                new ApiUnavailableException(response.StatusCode.ToString(), $"Api CBR не доступно");

        try
        {
            using Stream responseStream = await response.Content.ReadAsStreamAsync();
            XmlSerializer serializer = new XmlSerializer(typeof(RatesValueDTO));

            using (StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("windows-1251")))
            {
                RatesValueDTO valCurs = (RatesValueDTO)serializer.Deserialize(reader)!;
                return valCurs;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return null;
        }
    }

    private async Task<RateValueDTO?> GetValuteDataFromApiSingleAsync(string url)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        try
        {
            using HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode) throw new ApiUnavailableException(response.StatusCode.ToString(), $"Api CBR не доступно");

            using Stream responseStream = await response.Content.ReadAsStreamAsync();
            XmlSerializer serializer = new XmlSerializer(typeof(RateValueDTO));

            using (StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("windows-1251")))
            {
                RateValueDTO result = (RateValueDTO)serializer.Deserialize(reader)!;
                return result;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return null;
        }
    }
}
