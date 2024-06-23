using AutoMapper;
using Exchange.Entities;
using Exchange.Services.ValutaRate.Data.DTO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exchange.Services.ValutaRate.Data.Mapper;

public class VoluteProfile : Profile
{
    public VoluteProfile()
    {
        CreateMap<VoluteDTO, Volute>()
                   .ForMember(dest => dest.Idname, opt => opt.MapFrom(src => src.ID))
                   .ForMember(dest => dest.Numcode, opt => opt.MapFrom(src => src.NumCode))
                   .ForMember(dest => dest.Charcode, opt => opt.MapFrom(src => src.CharCode))
                   .ForMember(dest => dest.Nominal, opt => opt.MapFrom(src => src.Nominal))
                   .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                   .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Value))
                   .ForMember(dest => dest.Vunitrate, opt => opt.MapFrom(src => src.VunitRate))
                   .ForMember(dest => dest.Valcursid, opt => opt.Ignore())
                   .ForMember(dest => dest.Valcurs, opt => opt.Ignore())
                   .ForMember(dest => dest.Id, opt => opt.Ignore())
                   .ReverseMap()
                   .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.Idname))
                   .ForMember(dest => dest.NumCode, opt => opt.MapFrom(src => src.Numcode))
                   .ForMember(dest => dest.CharCode, opt => opt.MapFrom(src => src.Charcode))
                   .ForMember(dest => dest.Nominal, opt => opt.MapFrom(src => src.Nominal))
                   .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                   .ForMember(dest => dest.ValueString, opt => opt.Ignore()) // opt => opt.MapFrom(src => src.Value.ToString(CultureInfo.InvariantCulture)))
                   .ForMember(dest => dest.VunitRateString, opt => opt.Ignore()); //opt => opt.MapFrom(src => src.Vunitrate.ToString(CultureInfo.InvariantCulture)));

        CreateMap<RateValue, RateValueDTO>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date.ToString("dd.MM.yyyy")))
                .ForMember(dest => dest.Volute, opt => opt.MapFrom(src => src.Volutes))
                .ReverseMap()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => DateOnly.ParseExact(src.Date, "dd.MM.yyyy")))
                .ForMember(dest => dest.Volutes, opt => opt.MapFrom(src => src.Volute));
    }
}
