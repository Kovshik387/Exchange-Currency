namespace Exchange.ExchangeAccount;

using Exchange.Services.Settings;
using Exchange.ExchangeAccount.Configuration;
using Exchange.Services.Authorization;
using Exchange.Services.Account;

public static class Bootstrapper
{
    public static IServiceCollection RegisterService(this IServiceCollection services, IConfiguration configuration = null)
    {
        services.AddMainSettings()
            .AddLogSettings()
            .AddAuthSettings()
            .AddApiKeySettings()
            .AddSwaggerSettings()
            .AddAppJwt()
            .AddAuthService()
            .AddAccountService()
            ;

        return services;
    }
}
