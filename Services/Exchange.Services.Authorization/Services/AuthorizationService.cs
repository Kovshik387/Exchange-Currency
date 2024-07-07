using AutoMapper;
using Exchange.Account.Context;
using Exchange.Account.Context.Context;
using Exchange.Services.Authorization.Data.DTO;
using Exchange.Services.Authorization.Infrastructure;
using Exchange.Services.Authorization.Services.Validators;
using Exchange.Services.Authorization.Utils;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Exchange.Services.Authorization.Services;
/// <summary>
/// Реализация интерфейса <see cref="IAuthorizationService"/> аутентификации
/// </summary>
/// <param name="mapper"></param>
/// <param name="context"></param>
public class AuthorizationService : IAuthorizationService
{
    private readonly IMapper _mapper;
    private readonly IValidator<SignUpDTO> _signUpValidator;
    private readonly IValidator<SignInDTO> _signInValidator;
    private readonly AccountDbContext _context;
    private readonly IJwtUtils _jwtUtils;
    private readonly ILogger<AuthorizationService> _logger ;

    public AuthorizationService(IMapper mapper, IValidator<SignInDTO> signInValidator, IValidator<SignUpDTO> signUpValidator,
        AccountDbContext context, IJwtUtils jwtUtils, ILogger<AuthorizationService> logger)
    {
        _mapper = mapper; _signInValidator = signInValidator; _signUpValidator = signUpValidator;
        _context = context; _jwtUtils = jwtUtils; _logger = logger; 
    }

    public async Task<bool> IsEmptyAsync()
    {
        return !(await _context.Users.AnyAsync());
    }

    public async Task<AuthResponse<AuthDTO>> SignInAsync(SignInDTO model)
    {
        var validated = await _signInValidator.ValidateAsync(model);
        if (!validated.IsValid)
        {
            return new AuthResponse<AuthDTO>()
            {
                Data = null,
                ErrorMessage = validated.Errors.First().ErrorMessage
            };
        }

        var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == model.Email.ToUpper());

        if (user is null)
        {
            return new AuthResponse<AuthDTO>()
            {
                Data = null,
                ErrorMessage = "Invalid email."
            };
        }

        if (!BCrypt.Net.BCrypt.Verify(model.Password, user.Passwordhash))
        {
            return new AuthResponse<AuthDTO>()
            {
                Data = null,
                ErrorMessage = "Invalid password."
            };
        }

        var authResult = _jwtUtils.GenerateJwtToken(user.Id); authResult.Name = user.Name;

        user.Refreshtoken = authResult.RefreshToken;
        _context.Users.Update(user); await _context.SaveChangesAsync();

        return new AuthResponse<AuthDTO>()
        {
            Data = authResult,
            ErrorMessage = ""
        };
    }
    public async Task<AuthResponse<AuthDTO>> SignUpAsync(SignUpDTO model)
    {
        var validated = await _signUpValidator.ValidateAsync(model);
        if (!validated.IsValid)
        {
            return new AuthResponse<AuthDTO>()
            {
                Data = null,
                ErrorMessage = validated.Errors.First().ErrorMessage
            };
        }

        var userExist = await _context.Users.Where(x => x.Email == model.Email.ToUpper()).FirstOrDefaultAsync();
        if (userExist != null) return new AuthResponse<AuthDTO>()
        {
            Data = null,
            ErrorMessage = $"User account with email {model.Email} already exist."
        };

        var userGuid = Guid.NewGuid();

        var token = _jwtUtils.GenerateJwtToken(userGuid);
        var user = new User()
        {
            Id = userGuid,
            Email = model.Email.ToUpper(),
            Passwordhash = BCrypt.Net.BCrypt.HashPassword(model.Password, 10),
            Name = model.Name,
            Surname = model.Surname,
            Patronymic = model.Patronymic,
            Refreshtoken = token.RefreshToken,
        };

        var result = await _context.Users.AddAsync(user); await _context.SaveChangesAsync();

        if (result is null)
            throw new Exception($"Creating user account is wrong.");

        return new AuthResponse<AuthDTO>()
        {
            Data = new AuthDTO()
            {
                RefreshToken = token.RefreshToken,
                AccessToken = token.AccessToken,
                Id = userGuid,
                Name = user.Name
            },
            ErrorMessage = ""
        };
    }

    public async Task<AuthResponse<AuthDTO>> GetAccessTokenAsync(string refreshToken)
    {
        var idUser = _jwtUtils.GetUserByRefreshToken(refreshToken);

        if (idUser is null)
        {
            return new AuthResponse<AuthDTO>()
            {
                Data = null,
                ErrorMessage = "Invalid Refresh Token."
            };
        }

        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == Guid.Parse(idUser));

        if (user is null || user.Refreshtoken != refreshToken)
        {
            return new AuthResponse<AuthDTO>()
            {
                Data = null,
                ErrorMessage = "Invalid Refresh Token."
            };
        }

        var tokens = _jwtUtils.GenerateJwtToken(Guid.Parse(idUser));
        user.Refreshtoken = tokens.RefreshToken; await _context.SaveChangesAsync();

        return new AuthResponse<AuthDTO>()
        {
            Data = new AuthDTO()
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

        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == Guid.Parse(idUser));
        if (user is null || user.Refreshtoken != refreshToken)
        {
            return new AuthResponse<object>()
            {
                Data = null,
                ErrorMessage = "Invalid Refresh Token."
            };
        }

        user.Refreshtoken = ""; _context.Users.Update(user); await _context.SaveChangesAsync();
        return new AuthResponse<object>
        {
            Data = null,
            ErrorMessage = ""
        };
    }
}
