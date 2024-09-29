using Exchange.Services.Authorization.Data.DTO;

namespace Exchange.Services.Authorization.Infrastructure;

public interface IJwtUtils
{
    public AuthDto GenerateJwtToken(Guid guid);
    public string? GetUserByRefreshToken(string refreshToken);
    public string? GetExpireTime(string refreshToken);
}
