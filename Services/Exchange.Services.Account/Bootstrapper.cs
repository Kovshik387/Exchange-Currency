using Exchange.Services.Account.Infrastructure;
using Exchange.Services.Account.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Exchange.Services.Account
{
    public static class Bootstrapper
    {
        public static IServiceCollection AddAccountService(this IServiceCollection services)
        {
            services.AddTransient<IAccountService, AccountService>();
            return services;
        }

    }
}
