using AccountServiceProto;
using AutoMapper;
using Exchange.Services.Account.Data.DTO;

namespace Exchange.ExchangeAccount.Data.Mapper;

public class AccountGrpcProfile : Profile
{
    public AccountGrpcProfile()
    {
        CreateMap<AccountRequest, AccountDto>().ReverseMap();
    }
}