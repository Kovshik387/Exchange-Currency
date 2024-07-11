using Exchange.Services.Logger;
using Exchange.Services.RabbitMq;
using Exchange.Services.Settings;

namespace Exchange.WorkerDaily;

public static class Bootstrapper
{
    public static IServiceCollection RegisterAppServices(this IServiceCollection services)
    {
        services
            .AddAppLogger()
            .AddApiKeySettings()
            .AddRabbitMq()
            ;
        return services;
    }
}
