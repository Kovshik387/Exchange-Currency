using AutoMapper;
// using Exchange.Account.Context;
using Exchange.Services.Authorization.Data.DTO;
using Exchange.Services.Authorization.Data.Responses;

namespace Exchange.Services.Authorization.Data.Mapper;

public class AuthorizationProfile : Profile
{
    public AuthorizationProfile()
    {
        // CreateMap<User, AuthorizationResponse>().ReverseMap();
        CreateMap<AuthorizationResponse, SignInDto>().ReverseMap();
        // CreateMap<AuthDto, User>().ReverseMap();
    }
}
