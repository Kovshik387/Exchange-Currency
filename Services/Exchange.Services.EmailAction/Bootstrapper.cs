using Exchange.Services.EmailAction.Infrastructure;
using Exchange.Services.EmailAction.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Exchange.Services.EmailAction;
public static class Bootstrapper
{
    public static IServiceCollection AddEmailActions(this IServiceCollection services)
    {
        services.AddSingleton<IEmailAction, Services.EmailAction>();

        return services;
    }
}
