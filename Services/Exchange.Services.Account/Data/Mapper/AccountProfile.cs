using AutoMapper;
using Exchange.Account.Context;
using Exchange.Services.Account.Data.DTO;

namespace Exchange.Services.Account.Data.Mapper;

public class AccountProfile : Profile
{
    public AccountProfile() 
    {
        CreateMap<AccountDTO,User>().ReverseMap();
        CreateMap<FavoriteDTO, Favorite>().ReverseMap();
    }
}
