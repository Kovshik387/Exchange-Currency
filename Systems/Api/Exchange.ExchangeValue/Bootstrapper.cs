using Exchange.Services.Settings;
using Exchange.Services.Logger;

namespace Exchange.ExchangeValute;

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
