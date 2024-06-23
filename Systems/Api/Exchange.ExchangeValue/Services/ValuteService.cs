using Grpc.Core;
using ExchangeVoluteService;
using Exchange.Services.ValutaRate.Infrastructure;
using System.Globalization;
using AutoMapper;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Xml.Linq;
using Exchange.Services.ValutaRate.Data.DTO;

namespace Exchange.ExchangeValute.Services;

public class ValuteService(IVoluteRateService voluteRateService, IMapper mapper) : Volute.VoluteBase
{
    private readonly IVoluteRateService _voluteRateService = voluteRateService;
    private readonly IMapper _mapper = mapper;
    public override async Task<DailyValuteResponse> GetCurrentValue(DailyValuteRequest request, ServerCallContext context)
    {
        if (DateOnly.TryParseExact(request.Date, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly parsedDate))
        {
            return _mapper.Map<DailyValuteResponse>(await _voluteRateService.GetCursByDateAsync(parsedDate));
        }
        else
        {
            throw new RpcException(new Status(StatusCode.NotFound, "За заданную дату нет данных"));
        }
    }

    public override async Task<DynamicValueResponse> GetDynamicValue(DynamicValueRequest request, ServerCallContext context)
    {
        if (DateOnly.TryParseExact(request.Date1, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly parsedDate1) &&
                DateOnly.TryParseExact(request.Date2, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly parsedDate2))
        {
            var records = await _voluteRateService.GetCursListByDateAsync(parsedDate1, parsedDate2, request.Name);
            var response = new DynamicValueResponse();
            response.Record.AddRange(_mapper.Map<List<RecordResponse>>(records));
            return response;
        }
        else
        {
            throw new RpcException(new Status(StatusCode.NotFound, "За заданную дату нет данных"));
        }
    }
}
