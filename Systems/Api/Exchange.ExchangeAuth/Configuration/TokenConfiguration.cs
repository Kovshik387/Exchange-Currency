using Exchange.Services.Authorization.Infrastructure;
using Exchange.Services.Authorization.Utils;

namespace Exchange.ExchangeAuth.Configuration;

public static class TokenConfiguration
{
    public static IServiceCollection AddAppJwt(this IServiceCollection services)
    {
        return services.AddScoped<IJwtUtils, JwtUtils>();
    }
}