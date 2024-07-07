using Exchange.Services.RabbitMq.Infrastructure;
using Exchange.Services.Settings.SettingsConfigure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Exchange.Services.RabbitMq;

public static class Bootstrapper
{
    public static IServiceCollection AddRabbitMq(this IServiceCollection services, IConfiguration configuration = null)
    {
        var settings = Common.Settings.Settings.Load<RabbitMqSettings>("RabbitMq", configuration);
        services.AddSingleton(settings);

        services.AddSingleton<IRabbitMq, Exchange.Services.RabbitMq.RabbitMq.RabbitMq>();

        return services;
    }
}