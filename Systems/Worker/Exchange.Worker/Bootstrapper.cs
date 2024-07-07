using Exchange.Services.Logger;
using Exchange.Services.RabbitMq;
using Exchange.Worker.TaskExecutor;

namespace Exchange.Worker;

public static class Bootstrapper
{
    public static IServiceCollection RegisterAppServices(this IServiceCollection services)
    {
        services
            .AddAppLogger()
            .AddRabbitMq()
            ;
        return services;
    }
}
