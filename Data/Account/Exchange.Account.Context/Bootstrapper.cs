using Exchange.Account.Context.Context;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Exchange.Account.Context.Settings;
using Exchange.Account.Context.Factories;

namespace Exchange.Account.Context;

public static class Bootstrapper
{
    public static IServiceCollection AddAppDbContext(this IServiceCollection services, IConfiguration configuration = null)
    {
        var settings = Common.Settings.Settings.Load<DbSettings>("Database", configuration);
        services.AddSingleton(settings);

        var dbInitDelegate = DbContextOptionsFactory.Configure(settings.ConnectionString, settings.DatabaseType, true);

        services.AddDbContextFactory<AccountDbContext>(dbInitDelegate);
        return services;
    }
}
