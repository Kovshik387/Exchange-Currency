using AccountServiceProto;
using Exchange.Authorization.Context.Context;
using Exchange.Authorization.Entities;
using Exchange.Services.Authorization.Data.DTO;
using Exchange.Services.Authorization.Infrastructure;
using Exchange.Services.Settings.SettingsConfigure;
using FluentValidation;
using Grpc.Net.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Exchange.Services.Authorization.Services;
/// <summary>
/// Реализация интерфейса <see cref="IAuthorizationService"/> аутентификации
/// </summary>
public class AuthorizationService : IAuthorizationService
{
    private readonly IValidator<SignUpDto> _signUpValidator;
    private readonly IValidator<SignInDto> _signInValidator;
    private readonly AuthorizationDbContext _context;
    private readonly IJwtUtils _jwtUtils;
    private readonly ApiEndPointSettings _endPointSettings;
    private readonly ILogger<AuthorizationService> _logger;
    public AuthorizationService(IValidator<SignInDto> signInValidator, IValidator<SignUpDto> signUpValidator,
        AuthorizationDbContext context, IJwtUtils jwtUtils, ILogger<AuthorizationService> logger, ApiEndPointSettings endPointSettings)
    {
        _signInValidator = signInValidator; _signUpValidator = signUpValidator;
        _context = context; _jwtUtils = jwtUtils; _endPointSettings = endPointSettings;
        _logger = logger;
    }
    
    public async Task<bool> IsEmptyAsync()
    {
        return !await _context.Accounts.AnyAsync();
    }
    
    public async Task<AuthResponse<AuthDto>> SignInAsync(SignInDto model)
    {
        var validated = await _signInValidator.ValidateAsync(model);
        if (!validated.IsValid)
        {
            return new AuthResponse<AuthDto>()
            {
                Data = null,
                ErrorMessage = validated.Errors.First().ErrorMessage
            };
        }
        
        var user = await _context.Accounts
            .Include(account => account.Refreshes)
            .FirstOrDefaultAsync(x => x.EmailNormalized.Equals(model.Email.ToUpper()));
        
        if (user is null)
        {
            return new AuthResponse<AuthDto>()
            {
                Data = null,
                ErrorMessage = "Invalid email."
            };
        }
        
        if (!BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
        {
            return new AuthResponse<AuthDto>()
            {
                Data = null,
                ErrorMessage = "Invalid password."
            };
        }
        
        var authResult = _jwtUtils.GenerateJwtToken(user.Id);

        var device = user.Refreshes.FirstOrDefault(x => x.Device.Equals(model.Device));
        
        if (device is null)
        {
            user.Refreshes.Add(new RefreshToken()
            {
                Device = model.Device,
                Token = authResult.RefreshToken,
            });
        }
        else
        {
            device.Token = authResult.RefreshToken;
        }
        
        _context.Accounts.Update(user); await _context.SaveChangesAsync();

        return new AuthResponse<AuthDto>()
        {
            Data = authResult,
            ErrorMessage = ""
        };
    }
    public async Task<AuthResponse<AuthDto>> SignUpAsync(SignUpDto model)
    {
        var validated = await _signUpValidator.ValidateAsync(model);
        if (!validated.IsValid)
        {
            return new AuthResponse<AuthDto>()
            {
                Data = null,
                ErrorMessage = validated.Errors.First().ErrorMessage
            };
        }
        
        var accountExist = await _context.Accounts
            .Where(x => x.EmailNormalized == model.Email.ToUpper())
            .FirstOrDefaultAsync();
        
        if (accountExist is not null) return new AuthResponse<AuthDto>()
        {
            Data = null,
            ErrorMessage = $"User account with email {model.Email} already exist."
        };

        var userGuid = Guid.NewGuid();

        var token = _jwtUtils.GenerateJwtToken(userGuid);

        var account = new Exchange.Authorization.Entities.Account()
        {
            Id = userGuid,
            Email = model.Email,
            EmailNormalized = model.Email.ToUpper(),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password, 10),
        };
        
        using var channel = GrpcChannel.ForAddress(_endPointSettings.GrpcServerPath);
        var client = new AccountServiceProto.Account.AccountClient(channel);
        
        var response = await client.AddAccountAsync(new AccountRequest()
        {
            Id = userGuid.ToString(),
            Email = model.Email,
            Name = model.Name,
            Patronymic = model.Patronymic,
            Surname =  model.Surname,
        });

        if (!response.Success)
        {
            return new AuthResponse<AuthDto>()
            {
                Data = null,
                ErrorMessage = "Failed to create account. Try again later"
            };
        }
        
        account.Refreshes.Add(new RefreshToken()
        {
            Token = token.RefreshToken,
            Device = model.Device,
        });
        var result = await _context.Accounts.AddAsync(account); 
        
        await _context.SaveChangesAsync();

        if (result is null)
            throw new Exception($"Creating user account is wrong.");

        return new AuthResponse<AuthDto>()
        {
            Data = new AuthDto()
            {
                RefreshToken = token.RefreshToken,
                AccessToken = token.AccessToken,
                Id = userGuid,
            },
            ErrorMessage = ""
        };
    }

    public async Task<AuthResponse<AuthDto>> GetAccessTokenAsync(string refreshToken)
    {
        if (DateTime.Now > DateTime.Parse(_jwtUtils.GetExpireTime(refreshToken) ?? string.Empty))
        {
            this._logger.LogInformation(_jwtUtils.GetExpireTime(refreshToken));
            
            this._logger.LogInformation($"Токен просрочен: {refreshToken} | now {DateTime.Now}");

            return new AuthResponse<AuthDto>()
            {
                Data = null,
                ErrorMessage = "Invalid Refresh Token."
            };
        }
        
        var idUser = _jwtUtils.GetUserByRefreshToken(refreshToken);
        
        if (idUser is null)
        {
            return new AuthResponse<AuthDto>()
            {
                Data = null,
                ErrorMessage = "Invalid Refresh Token."
            };
        }

        var user = await _context.Accounts
            .Include(account => account.Refreshes)
            .FirstOrDefaultAsync(x => x.Id == Guid.Parse(idUser));

        var tokenExist = user?.Refreshes.FirstOrDefault(x => x.Token.Equals(refreshToken)); 
        
        if (tokenExist is null)
        {
            return new AuthResponse<AuthDto>()
            {
                Data = null,
                ErrorMessage = "Invalid Refresh Token."
            };
        }
        
        var tokens = _jwtUtils.GenerateJwtToken(Guid.Parse(idUser));
        tokenExist.Token = tokens.RefreshToken; await _context.SaveChangesAsync();

        return new AuthResponse<AuthDto>()
        {
            Data = new AuthDto()
            {
                Id = Guid.Parse(idUser),
                AccessToken = tokens.AccessToken,
                RefreshToken = tokens.RefreshToken,
            },
            ErrorMessage = ""
        };
    }

    public async Task<AuthResponse<object>> Logout(string refreshToken)
    {
        var idUser = _jwtUtils.GetUserByRefreshToken(refreshToken);
        if (idUser is null)
        {
            return new AuthResponse<object>()
            {
                Data = null,
                ErrorMessage = "Invalid Refresh Token."
            };
        }

        var user = await _context.Accounts
            .Include(account => account.Refreshes)
            .FirstOrDefaultAsync(x => x.Id == Guid.Parse(idUser));

        var refresh = user?.Refreshes.FirstOrDefault(x => x.Token == refreshToken); 
        
        if (refresh is null || user is null)
        {
            return new AuthResponse<object>()
            {
                Data = null,
                ErrorMessage = "Invalid Refresh Token."
            };
        }
        
        user.Refreshes.Remove(refresh); await _context.SaveChangesAsync();
        
        return new AuthResponse<object>
        {
            Data = null,
            ErrorMessage = ""
        };
    }
    //TODO 
    // доделать 
    public async Task<AuthResponse<object>> LogoutAll(string refreshToken)
    {
        var idUser = _jwtUtils.GetUserByRefreshToken(refreshToken);
        if (idUser is null)
        {
            return new AuthResponse<object>()
            {
                Data = null,
                ErrorMessage = "Invalid Refresh Token."
            };
        }

        var user = await _context.Accounts
            .Include(account => account.Refreshes)
            .FirstOrDefaultAsync(x => x.Id == Guid.Parse(idUser));
        
        if (user?.Refreshes.FirstOrDefault(x => x.Token == refreshToken) is null)
        {
            return new AuthResponse<object>()
            {
                Data = null,
                ErrorMessage = "Invalid Refresh Token."
            };
        }

        // user.Refreshtoken = ""; _context.Users.Update(user); await _context.SaveChangesAsync();
        return new AuthResponse<object>
        {
            Data = null,
            ErrorMessage = ""
        };
    }
}
