using Exchange.Services.Storage.Infrastructure;
using Exchange.Services.Storage.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Exchange.Services.Storage;

public static class Bootstrapper
{
    public static IServiceCollection AddStorageService(this IServiceCollection services)
    {
        return services.AddTransient<IStorageService, StorageService>();
    }
}