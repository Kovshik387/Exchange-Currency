using Exchange.Services.Cache;
using Exchange.Services.Settings;

namespace Exchange.ExchangeCache;

public static class Bootstrapper
{
    public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration = null)
    {
        return services
            .AddMainSettings()
            .AddApiPathsSettings()
            .AddCacheService()
            .AddReddisSettings();
    }
}
