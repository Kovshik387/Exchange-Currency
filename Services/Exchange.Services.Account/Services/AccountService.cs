using AutoMapper;
using Castle.Core.Logging;
using Exchange.Account.Context;
using Exchange.Account.Context.Context;
using Exchange.Services.Account.Data.DTO;
using Exchange.Services.Account.Data.Responses;
using Exchange.Services.Account.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Exchange.Services.Account.Services;

public class AccountService : IAccountService
{
    private readonly AccountDbContext _context;
    private readonly ILogger<AccountService> _logger;
    private readonly IMapper _mapper;
    public AccountService(AccountDbContext context, ILogger<AccountService> logger, IMapper mapper) => (_context, _logger,_mapper) = (context, logger,mapper);

    public async Task<IList<AccountDTO>> GetAccountsAcceptedAsync()
    {
        return _mapper.Map<IList<AccountDTO>>(await _context.Users.Where(x => x.Accept.Equals(true)).ToListAsync());
    }

    public async Task<AccountResponse<AccountDTO>> GetAccountByIdAsync(Guid id)
    {
        var data = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        if (data is null) return ErrorResponse("User is not found");

        return new AccountResponse<AccountDTO>
        {
            Data = _mapper.Map<AccountDTO>(data),
            ErrorMessage = ""
        };
    }

    public async Task<AccountResponse<AccountDTO>> AddFavoriteVoluteAsync(Guid id,FavoriteDTO favoriteDTO)
    {
        var data = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        if (data is null) return ErrorResponse("User is not found");

        if (data.Favorites.FirstOrDefault(x => x.Volute.Equals(favoriteDTO.Volute)) is not null)
        {
            return ErrorResponse("Volute is already exist");
        }

        data.Favorites.Add(_mapper.Map<Favorite>(favoriteDTO)); await _context.SaveChangesAsync();

        return new AccountResponse<AccountDTO>
        {
            Data = _mapper.Map<AccountDTO>(data),
            ErrorMessage = ""
        };
    }

    public async Task<AccountResponse<AccountDTO>> DeleteFavoriteVoluteAsync(Guid id,FavoriteDTO favoriteDTO)
    {
        var data = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        if (data is null) return ErrorResponse("User is not found");

        var dataVolute = await _context.Favorites.FirstOrDefaultAsync(x => x.Iduser.Equals(id) && x.Volute.Equals(favoriteDTO.Volute));
        if (dataVolute is null) return ErrorResponse("Volute is not found");


        data.Favorites.Remove(dataVolute); await _context.SaveChangesAsync();

        return new AccountResponse<AccountDTO>
        {
            Data = _mapper.Map<AccountDTO>(data),
            ErrorMessage = ""
        };
    }

    public async Task<AccountResponse<AccountDTO>> ChangeForwardingAsync(Guid id)
    {
        var data = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        if (data is null) return ErrorResponse("User is not found");

        data.Accept = !data.Accept; await _context.SaveChangesAsync();

        return new AccountResponse<AccountDTO> { Data = null, ErrorMessage = "" };
    }

    private AccountResponse<AccountDTO> ErrorResponse(string message)
    {
        return new AccountResponse<AccountDTO>
        {
            Data = null,
            ErrorMessage = message
        };
    }
}
