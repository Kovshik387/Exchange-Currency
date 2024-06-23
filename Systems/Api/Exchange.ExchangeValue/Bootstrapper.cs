using Exchange.Services.Settings;
using Exchange.Services.Logger;
using Exchange.Services.ValutaRate;

namespace Exchange.ExchangeVolute;

public static class Bootstrapper
{
    public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration = null)
    {
        services.AddMainSettings()
            .AddLogSettings()
            .AddAppLogger()
            .AddValuteService()
            ;

        return services;
    }
}
