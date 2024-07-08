using Exchange.Services.Logger;
using Exchange.Services.MessageSendler;
using Exchange.Services.RabbitMq;
using Exchange.Services.Settings;
using Exchange.Worker.TaskExecutor;

namespace Exchange.Worker;

public static class Bootstrapper
{
    public static IServiceCollection RegisterAppServices(this IServiceCollection services)
    {
        services
            .AddAppLogger()
            .AddRabbitMq()
            .AddEmailSettings()
            .AddMessageService()
            ;
        return services;
    }
}
