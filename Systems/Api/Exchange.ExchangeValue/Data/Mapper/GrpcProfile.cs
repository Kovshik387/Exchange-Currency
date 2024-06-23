using AutoMapper;
using Exchange.Services.ValutaRate.Data.DTO;
using ExchangeVoluteService;

namespace Exchange.ExchangeValute.Data.Mapper;

public class GrpcProfile : Profile
{
    public GrpcProfile()
    {
        CreateMap<RateValueDTO, DailyValuteResponse>().ReverseMap();
        CreateMap<VoluteDTO, VoluteResponse>().ReverseMap();
        CreateMap<RecordDTO, RecordResponse>().ReverseMap();
    }
}
