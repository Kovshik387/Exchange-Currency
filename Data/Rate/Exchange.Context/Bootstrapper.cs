using Exchange.Context.Context;
using Exchange.Context.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Exchange.Context
{
    public static class Bootstrapper
    {
        public static IServiceCollection AddAppDbContext(this IServiceCollection services, IConfiguration configuration = null)
        {
            var settings = Common.Settings.Settings.Load<DbSettings>("Database", configuration);
            services.AddSingleton(settings);

            var dbInitDelegate = DbContextOptionsFactory.Configure(settings.ConnectionString, settings.DatabaseType, true);

            services.AddDbContextFactory<RateDbContext>(dbInitDelegate);
            return services;
        }
    }
}
