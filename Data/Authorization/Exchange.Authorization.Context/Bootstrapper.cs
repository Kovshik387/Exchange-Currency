using Exchange.Authorization.Context.Context;
using Exchange.Authorization.Context.Factories;
using Exchange.Authorization.Context.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Exchange.Authorization.Context;

public static class Bootstrapper
{
    public static IServiceCollection AddAppDbContext(this IServiceCollection services, IConfiguration configuration = null)
    {
        var settings = Common.Settings.Settings.Load<DbSettings>("Database", configuration);
        services.AddSingleton(settings);

        var dbInitDelegate = DbContextOptionsFactory.Configure(settings.ConnectionString, settings.DatabaseType, true);

        services.AddDbContextFactory<AuthorizationDbContext>(dbInitDelegate);
        return services;
    }
}