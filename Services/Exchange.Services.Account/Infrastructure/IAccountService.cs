using Exchange.Services.Account.Data.DTO;
using Exchange.Services.Account.Data.Responses;

namespace Exchange.Services.Account.Infrastructure;

public interface IAccountService
{
    public Task<IList<AccountDto>> GetAccountsAcceptedAsync();
    public Task<AccountResponse<AccountDto>> GetAccountByIdAsync(Guid id);
    public Task<AccountResponse<AccountDto>> AddFavoriteVoluteAsync(Guid id, FavoriteDto favoriteDto);
    public Task<AccountResponse<AccountDto>> DeleteFavoriteVoluteAsync(Guid id, FavoriteDto favoriteDto);
    public Task<AccountResponse<AccountDto>> ChangeForwardingAsync(Guid id);
    public Task<AccountResponse<bool>> AddAccountAsync(AccountDto accountDto);
}
