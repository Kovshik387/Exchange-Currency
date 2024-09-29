using Exchange.Services.Authorization.Infrastructure;
using Exchange.Services.Settings.SettingsConfigure;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Exchange.Services.Authorization.Data.DTO;

namespace Exchange.Services.Authorization.Utils;

public class JwtUtils : IJwtUtils
{
    private readonly AuthSettings _settings;
    private readonly ILogger<JwtUtils> _logger;

    public JwtUtils( AuthSettings settings, ILogger<JwtUtils> logger) => 
        (_settings, _logger) = (settings, logger);

    public AuthDto GenerateJwtToken(Guid guid)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var claims = new List<Claim>() { new Claim(ClaimTypes.Name, guid.ToString()) };

        var accessTokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_settings.AccessTokenLifetimeMinutes),
            Issuer = _settings.Issuer,
            Audience = _settings.Audience,
            SigningCredentials = new SigningCredentials(_settings.SymmetricSecurityKeyAccess,
                SecurityAlgorithms.HmacSha256),
        };

        var refreshTokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(_settings.RefreshTokenLifetimeDays),
            Issuer = _settings.Issuer,
            Audience = _settings.Audience,
            SigningCredentials = new SigningCredentials(_settings.SymmetricSecurityKeyRefresh,
                SecurityAlgorithms.HmacSha256Signature),
        };

        var authDto = new AuthDto()
        {
            Id = guid,
            AccessToken = tokenHandler.WriteToken(tokenHandler.CreateToken(accessTokenDescriptor)),
            RefreshToken = tokenHandler.WriteToken(tokenHandler.CreateToken(refreshTokenDescriptor))
        };

        return authDto;
    }

    public string? GetExpireTime(string refreshToken)
    {
        var claims = GetClaimsFromToken(refreshToken);

        return claims?.Claims.First(x => x.Type.Equals("exp")).Value;
    }

    public string? GetUserByRefreshToken(string refreshToken)
    {
        var claims = GetClaimsFromToken(refreshToken);

        return claims is not null ? claims.Claims.First(x => x.Type.Equals(ClaimTypes.Name)).Value : null;
    }

    private ClaimsPrincipal? GetClaimsFromToken(string refreshToken)
    {
        var tokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidIssuer = _settings.Issuer,
            ValidateAudience = true,
            ValidAudience = _settings.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = _settings.SymmetricSecurityKeyRefresh
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            return tokenHandler.ValidateToken(refreshToken, tokenValidationParameters, out var validatedToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return null;
        }
    }
}
