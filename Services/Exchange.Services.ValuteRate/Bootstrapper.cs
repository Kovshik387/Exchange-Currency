using Exchange.Services.ValutaRate.Infrastructure;
using Exchange.Services.ValutaRate.Services;
using Microsoft.Extensions.DependencyInjection;


namespace Exchange.Services.ValutaRate;

/// <summary>
/// Регистрация сервиса манипуляций с данными курсов валют
/// </summary>
public static class Bootstrapper
{
    public static IServiceCollection AddValuteService(this IServiceCollection services)
    {
        return services.AddTransient<IVoluteRateService, VoluteRateService>();
    }
}
