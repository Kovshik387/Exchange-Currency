using Exchange.Services.Settings;
using Exchange.Services.Logger;

namespace Exchange.ExchangeVolute;

public static class Bootstrapper
{
    public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration = null)
    {
        services.AddMainSettings()
            .AddLogSettings()
            .AddAppLogger()
            ;

        return services;
    }
}
