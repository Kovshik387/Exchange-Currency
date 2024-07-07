using Exchange.Services.Account.Data.DTO;
using Exchange.Services.Account.Data.Responses;

namespace Exchange.Services.Account.Infrastructure;

public interface IAccountService
{
    public Task<IList<AccountDTO>> GetAccountsAcceptedAsync();
    public Task<AccountResponse<AccountDTO>> GetAccountByIdAsync(Guid id);
    public Task<AccountResponse<AccountDTO>> AddFavoriteVoluteAsync(Guid id, FavoriteDTO favoriteDTO);
    public Task<AccountResponse<AccountDTO>> DeleteFavoriteVoluteAsync(Guid id, FavoriteDTO favoriteDTO);
    public Task<AccountResponse<AccountDTO>> ChangeForwardingAsync(Guid id);
}
