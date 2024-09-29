namespace Exchange.Services.Authorization;

using Exchange.Services.Authorization.Infrastructure;
using Exchange.Services.Authorization.Services;
using Microsoft.Extensions.DependencyInjection;
public static class Bootstrapper
{
    public static IServiceCollection AddAuthService(this IServiceCollection services)
    {
        services.AddScoped<IAuthorizationService, AuthorizationService>();
        return services;
    }
}
