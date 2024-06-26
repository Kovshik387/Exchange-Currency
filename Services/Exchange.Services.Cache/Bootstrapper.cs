using Exchange.Services.Cache.Infrastructure;
using Exchange.Services.Cache.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Exchange.Services.Cache;

public static class Bootstrapper
{
    public static IServiceCollection AddCacheService(this IServiceCollection services)
    {
        return services.AddTransient<ICacheService, CacheService>();
    }
}
