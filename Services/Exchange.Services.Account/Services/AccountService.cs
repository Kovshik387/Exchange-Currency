using AutoMapper;
using Castle.Core.Logging;
using Exchange.Account.Context;
using Exchange.Account.Context.Context;
using Exchange.Services.Account.Data.DTO;
using Exchange.Services.Account.Data.Responses;
using Exchange.Services.Account.Infrastructure;
using Exchange.Services.Settings.SettingsConfigure;
using Google.Protobuf;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StorageServiceProto;

namespace Exchange.Services.Account.Services;

public class AccountService : IAccountService
{
    private readonly AccountDbContext _context;
    private readonly ILogger<AccountService> _logger;
    private readonly IMapper _mapper;
    private readonly ApiEndPointSettings _endPointSettings;
    
    public AccountService(AccountDbContext context, ILogger<AccountService> logger, IMapper mapper, ApiEndPointSettings endPointSettings)
    => (_context, _logger, _mapper,_endPointSettings) = (context, logger, mapper,endPointSettings);

    public async Task<IList<AccountDto>> GetAccountsAcceptedAsync()
    {
        return _mapper.Map<IList<AccountDto>>(await _context.Users.Where(x => x.Accept.Equals(true)).ToListAsync());
    }

    public async Task<AccountResponse<string>> UploadImageAsync(string id, IFormFile file)
    {
        using var channel = GrpcChannel.ForAddress(_endPointSettings.GrpcServerPath);
        var client = new StorageService.StorageServiceClient(channel);

        await using MemoryStream memoryStream = new();
        await file.CopyToAsync(memoryStream);
        
        var response = await client.UploadImageAsync(new UploadImageRequest()
        {
            Image = ByteString.CopyFrom(memoryStream.ToArray()),
            ImageFormat = file.ContentType,
            UserId = id
        });
        
        return new AccountResponse<string>()
        {
            Data = response.Url,
            ErrorMessage = response.ErrorMessage,
        };
    }
    
    public async Task<AccountResponse<AccountDto>> GetAccountByIdAsync(Guid id)
    {
        var data = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        if (data is null) return ErrorResponse("User is not found");
        
        var result = _mapper.Map<AccountDto>(data);
        using var channel = GrpcChannel.ForAddress(_endPointSettings.GrpcServerPath);
        var client = new StorageService.StorageServiceClient(channel);
        result.Url = (await client.GetImageAsync(new GetImageRequest()
        {
            UserId = id.ToString(),
        })).Url;
        
        return new AccountResponse<AccountDto>
        {
            Data = _mapper.Map<AccountDto>(result),
            ErrorMessage = ""
        };
    }

    public async Task<AccountResponse<AccountDto>> AddFavoriteVoluteAsync(Guid id,FavoriteDto favoriteDto)
    {
        var data = await _context.Users.Include(user => user.Favorites).FirstOrDefaultAsync(x => x.Id == id);
        if (data is null) return ErrorResponse("User is not found");

        if (data.Favorites.FirstOrDefault(x => x.Volute.Equals(favoriteDto.Volute)) is not null)
        {
            return ErrorResponse("Volute is already exist");
        }

        data.Favorites.Add(_mapper.Map<Favorite>(favoriteDto)); await _context.SaveChangesAsync();

        return new AccountResponse<AccountDto>
        {
            Data = _mapper.Map<AccountDto>(data),
            ErrorMessage = ""
        };
    }

    public async Task<AccountResponse<AccountDto>> DeleteFavoriteVoluteAsync(Guid id,FavoriteDto favoriteDto)
    {
        var data = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        if (data is null) return ErrorResponse("User is not found");

        var dataVolute = await _context.Favorites.FirstOrDefaultAsync(x => x.Iduser.Equals(id) && x.Volute.Equals(favoriteDto.Volute));
        if (dataVolute is null) return ErrorResponse("Volute is not found");


        data.Favorites.Remove(dataVolute); await _context.SaveChangesAsync();

        return new AccountResponse<AccountDto>
        {
            Data = _mapper.Map<AccountDto>(data),
            ErrorMessage = ""
        };
    }

    public async Task<AccountResponse<AccountDto>> ChangeForwardingAsync(Guid id)
    {
        var data = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        if (data is null) return ErrorResponse("User is not found");

        data.Accept = !data.Accept; await _context.SaveChangesAsync();

        return new AccountResponse<AccountDto> { Data = null, ErrorMessage = "" };
    }

    public async Task<AccountResponse<bool>> AddAccountAsync(AccountDto accountDto)
    {
        try
        {
            _context.Users.Add(new User()
            {
                Id = accountDto.Id,
                Name = accountDto.Name,
                Email = accountDto.Email,
                Surname = accountDto.Surname,
                Patronymic = accountDto.Patronymic,
                Accept = accountDto.Accept,
            });
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return new AccountResponse<bool> { Data = false, ErrorMessage = $"{e.Message}" };   
        }

        return new AccountResponse<bool>()
        {
            Data = true,
            ErrorMessage = ""
        };
    }

    private static AccountResponse<AccountDto> ErrorResponse(string message)
    {
        return new AccountResponse<AccountDto>
        {
            Data = null,
            ErrorMessage = message
        };
    }
}
