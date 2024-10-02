using Exchange.ExchangeAuth.Configuration;
using Exchange.Services.Authorization;
using Exchange.Services.Settings;

namespace Exchange.ExchangeAuth;

public static class Bootstrapper
{
    public static IServiceCollection RegisterService(this IServiceCollection services, IConfiguration configuration = null)
    {
        services.AddMainSettings()
            .AddLogSettings()
            .AddAuthSettings()
            .AddApiKeySettings()
            .AddApiPathsSettings()
            .AddSwaggerSettings()
            .AddAppJwt()
            ;

        return services;
    }
}