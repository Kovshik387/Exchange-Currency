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
using System.Globalization;
using System.Text;
using System.Xml.Serialization;

namespace Exchange.Services.ValutaRate.Services;
/// <summary>
/// Реализация интерфейса <see cref="IVoluteRateService"/> курсов валют
/// </summary>
/// <param name="context"></param>
/// <param name="mapper"></param>
/// <param name="logger"></param>
public class VoluteRateService : IVoluteRateService
{
    private readonly string urlDaily = "http://www.cbr.ru/scripts/XML_daily.asp";
    private readonly string urlInterval = "http://www.cbr.ru/scripts/XML_dynamic.asp";
    private readonly int _voluteTypeSize = 42;
    private readonly RateDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<VoluteRateService> _logger;

    public VoluteRateService(RateDbContext context, IMapper mapper, ILogger<VoluteRateService> logger)
    {
        this._context = context; this._mapper = mapper; this._logger = logger;
    }

    public async Task<RateValueDTO?> GetCursByDateAsync(DateOnly? date = null)
    {
        _logger.LogInformation("Date:\t" + date);

        var data = await _context.RateValues.FirstOrDefaultAsync(x => x.Date.Equals(date));
        
        if (data is null)
        {
            var result = await GetValuteDataFromApiAsync<RateValueDTO>(
                date is null ? urlDaily : urlDaily + "?date_req=" + date.ToString()!.Replace(".","/")
            );

            if (result is null) return result;

            var record = _mapper.Map<RateValue>(result);
            if (await _context.RateValues.FirstOrDefaultAsync(x => x.Date.Equals(record.Date)) is not null) return result;

            await _context.RateValues.AddAsync(record); await _context.SaveChangesAsync();

            _logger.LogInformation("Запись данных в бд");
            return result;
        }
        else
        {
            if (data.Name == "No data")
            {

            }

            _logger.LogInformation($"count: {data.Volutes.Count}");
            if (data.Volutes.Count < _voluteTypeSize)
            {
                var result = await GetValuteDataFromApiAsync<RateValueDTO>(
                    date is null ? urlDaily : urlDaily + "?date_req=" + date.ToString()!.Replace(".", "/")
                );
                
                if (result!.Volute is null) return result;
                
                var newVolutes = _mapper.Map<List<Volute>>(result.Volute).Except(data.Volutes);
                
                foreach(var item in newVolutes)
                {
                    data.Volutes.Add(item);
                }
                await _context.SaveChangesAsync();
                _logger.LogInformation("Данные дозаписаны");
            }

            _logger.LogInformation("Данные из бд");
            return _mapper.Map<RateValueDTO>(data);
        }
    }

    public async Task<IList<RecordDTO>> GetCursListByDateAsync(DateOnly date1, DateOnly date2, string nameVal)
    {
        var existingRecords = await _context.Volutes
            .Where(v => v.Idname.Equals(nameVal) && v.Valcurs.Date >= date1 && v.Valcurs.Date <= date2)
            .ToListAsync();

        var existingDates = existingRecords.Select(v => v.Valcurs.Date).ToHashSet(); 

        var allDates = Enumerable.Range(0, date2.DayNumber - date1.DayNumber + 1)
                                 .Select(offset => date1.AddDays(offset))
                                 .ToList();

        var missingDates = allDates.Except(existingDates).ToList();

        var result = new List<RecordDTO>();

        var voluteTemplate = await _context.Volutes.FirstOrDefaultAsync(x => x.Idname.Equals(nameVal));

        if (missingDates.Count != 0)
        {
            var missingDateRanges = SplitDatesIntoRanges(missingDates);

            foreach (var dateRange in missingDateRanges)
            {
                var apiResult = await GetValuteDataFromApiAsync<RatesValueDTO>($"{urlInterval}?date_req1={dateRange.Item1}&" +
                    $"date_req2={dateRange.Item2}&VAL_NM_RQ={nameVal}");
                _logger.LogInformation($"Запрос недостающих дат: {dateRange.Item1}\t|\t{dateRange.Item2}");

                if (apiResult == null || apiResult.Records.Count == 0)
                {
                    foreach (var date in missingDates.Where(d => d >= dateRange.Item1 && d <= dateRange.Item2))
                    {
                        await AddEmptyData(date, nameVal);
                    }
                    continue;
                }

                foreach (var item in apiResult.Records)
                {
                    var recordDate = DateOnly.Parse(item.Date);
                    item.Name = voluteTemplate!.Name;
                    result.Add(item);

                    if (!existingDates.Contains(recordDate))
                    {
                        var rateValue = await _context.RateValues.FirstOrDefaultAsync(rv => rv.Date == recordDate);
                        if (rateValue == null)
                        {
                            rateValue = new RateValue
                            {
                                Date = recordDate,
                                Name = apiResult.Name
                            };
                            _context.RateValues.Add(rateValue);
                            await _context.SaveChangesAsync();
                        }

                        var volute = new Volute
                        {
                            Idname = item.Id,
                            Nominal = item.Nominal,
                            Value = item.Value,
                            Vunitrate = item.VunitRate,
                            Valcurs = rateValue,
                            Charcode = voluteTemplate!.Charcode,
                            Name = voluteTemplate.Name,
                            Numcode = voluteTemplate.Numcode,
                        };
                        await _context.Volutes.AddAsync(volute);
                        
                        continue;
                    }

                }
                var apiDates = apiResult.Records.Select(r => DateOnly.Parse(r.Date)).ToHashSet();
                var missingApiDates = missingDates
                    .Where(d => d >= dateRange.Item1 && d <= dateRange.Item2 && !apiDates.Contains(d))
                    .ToList();

                foreach (var missingDate in missingApiDates)
                {
                    await AddEmptyData(missingDate, nameVal);
                }
            }
            await _context.SaveChangesAsync();
        }
        else
        {
            _logger.LogInformation("Данные из бд");
        }

        var combinedResult = existingRecords.Where(x => x.Valcurs.Name != "No data").Select(x => new RecordDTO
        {
            Date = x.Valcurs.Date.ToString("dd.MM.yyyy"),
            Id = x.Idname,
            Name = x.Name,
            Nominal = x.Nominal,
            Value = x.Value,
            VunitRate = x.Vunitrate,
        }).ToList();

        combinedResult.AddRange(result);

        return [.. combinedResult.OrderBy(x => DateOnly.Parse(x.Date))];
    }

    private List<(DateOnly, DateOnly)> SplitDatesIntoRanges(List<DateOnly> dates)
    {
        var ranges = new List<(DateOnly, DateOnly)>();
        
        if (dates.Count == 0) return ranges;

        dates.Sort();

        var startDate = dates.First();
        var endDate = startDate;

        foreach (var date in dates.Skip(1))
        {
            if (date.DayNumber == endDate.DayNumber + 1)
            {
                endDate = date;
            }
            else
            {
                ranges.Add((startDate, endDate));
                startDate = date;
                endDate = startDate;
            }
        }

        ranges.Add((startDate, endDate));
        return ranges;
    }

    private async Task<TData?> GetValuteDataFromApiAsync<TData>(string url)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        try
        {
            using HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode) throw new ApiUnavailableException(response.StatusCode.ToString(), $"Api CBR не доступно");

            using Stream responseStream = await response.Content.ReadAsStreamAsync();
            XmlSerializer serializer = new XmlSerializer(typeof(TData));

            using StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("windows-1251"));
            var result = (TData)serializer.Deserialize(reader)!;
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return default;
        }
    }

    private async Task AddEmptyData(DateOnly date, string nameVal)
    {
        var data = await _context.RateValues.FirstOrDefaultAsync(x => x.Date == date)
                ?? new RateValue { Date = date, Name = "No data" };

        if (data.Id == 0)
        {
            _context.RateValues.Add(data);
            await _context.SaveChangesAsync();
        }

        //var voluteStub = new Volute
        //{
        //    Idname = nameVal,
        //    Nominal = 0,
        //    Value = 0,
        //    Vunitrate = 0,
        //    Valcurs = data,
        //    Charcode = "No data",
        //    Name = "No data",
        //    Numcode = 0,
        //};

        //_context.Volutes.Add(voluteStub);
        await _context.SaveChangesAsync();
    }
}
