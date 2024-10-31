using Exchange.Services.Settings;
using Exchange.Services.Storage;

namespace Exchange.ExchangeStorage;

public static class Bootstrapper
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddMainSettings()
            .AddStorageService()
            ;
        return services;
    }
}