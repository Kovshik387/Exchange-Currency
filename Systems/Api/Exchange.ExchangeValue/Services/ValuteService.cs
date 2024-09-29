using Grpc.Core;
using ExchangeServiceProto;
using Exchange.Services.ValutaRate.Infrastructure;
using System.Globalization;
using AutoMapper;

namespace Exchange.ExchangeValute.Services;

public class ValuteService : Volute.VoluteBase
{
    private readonly IVoluteRateService _voluteRateService;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;
    private const string Locale = "ru-RU";
    private const string DateStringFormat = "dd.MM.yyyy";

    public ValuteService(IVoluteRateService voluteRateService, IMapper mapper, ILogger<ValuteService> logger)
    {
        this._mapper = mapper; this._voluteRateService = voluteRateService; this._logger = logger;
    }
    public override async Task<DailyValuteResponse?> GetCurrentValue(DailyValuteRequest request, ServerCallContext context)
    {
        if (!DateOnly.TryParseExact(request.Date, DateStringFormat, CultureInfo.GetCultureInfo(Locale),
                DateTimeStyles.None, out var parsedDate)) return null;
        _logger.LogInformation($"Date: {request.Date}|\tParsed Date: {parsedDate}");
        return _mapper.Map<DailyValuteResponse>(await _voluteRateService.GetCursByDateAsync(parsedDate));
    }

    public override async Task<DynamicValueResponse> GetDynamicValue(DynamicValueRequest request, ServerCallContext context)
    {
        DateOnly.TryParseExact(request.Date1, DateStringFormat, CultureInfo.GetCultureInfo(Locale), DateTimeStyles.None, out var parsedDate1);
        DateOnly.TryParseExact(request.Date2, DateStringFormat, CultureInfo.GetCultureInfo(Locale), DateTimeStyles.None, out var parsedDate2);
        
        var records = await _voluteRateService.GetCursListByDateAsync(parsedDate1, parsedDate2, request.Name);
        
        var response = new DynamicValueResponse();
        response.Record.AddRange(_mapper.Map<List<RecordResponse>>(records));
        
        if (response.Record.Count.Equals(0)) throw new RpcException(new Status(StatusCode.NotFound, "За заданную дату нет данных"));
        
        _logger.LogInformation($"name: {response.Record[0].Name}");
        return response;
    }
}
