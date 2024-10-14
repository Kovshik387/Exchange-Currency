using Exchange.Services.StorageService.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Exchange.Services.StorageService;

public static class Bootstrapper
{
    public static IServiceCollection AddStorageService(this IServiceCollection services)
    {
        return services.AddTransient<IStorageService, Services.StorageService>();
    }
}