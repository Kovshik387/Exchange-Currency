using AccountServiceProto;
using AutoMapper;
using Exchange.Services.Account.Data.DTO;
using Exchange.Services.Account.Infrastructure;
using Grpc.Core;

namespace Exchange.ExchangeAccount.Services;

public class AccountService : AccountServiceProto.Account.AccountBase
{
    private readonly IMapper _mapper;
    private readonly ILogger _logger;
    private readonly IAccountService _accountService;

    private AccountService(IMapper mapper, ILogger<AccountService> logger, IAccountService accountService)
    {
        _mapper = mapper; _logger = logger;
        _accountService = accountService;
    }

    public override async Task<AccountResponse> AddAccount(AccountRequest request, ServerCallContext context)
    {
        var result = await _accountService.AddAccountAsync(_mapper.Map<AccountDto>(request));
        _logger.LogInformation($"Add new account: {result.Data}");
        return new AccountResponse()
        {
            Success = result.Data,
            ErrorMessage = result.ErrorMessage,
        };
    }
}