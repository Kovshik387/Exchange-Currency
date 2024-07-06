namespace Exchange.ExchangeAccount;

using Exchange.Services.Settings;
using Exchange.ExchangeAccount.Configuration;
using Exchange.Services.Authorization;

public static class Bootstrapper
{
    public static IServiceCollection RegisterService(this IServiceCollection services, IConfiguration configuration = null)
    {
        services.AddMainSettings()
            .AddLogSettings()
            .AddAuthSettings()
            .AddSwaggerSettings()
            .AddAppJwt()
            .AddUserAccountService()
            ;



        return services;
    }
}
