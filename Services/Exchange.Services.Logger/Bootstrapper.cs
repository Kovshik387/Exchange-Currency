using Exchange.Services.Logger.Logger;
using Microsoft.Extensions.DependencyInjection;

namespace Exchange.Services.Logger
{
    public static class Bootstrapper
    {
        public static IServiceCollection AddAppLogger(this IServiceCollection services)
        {
            return services
                .AddSingleton<IAppLogger, AppLogger>();
        }
    }
}
