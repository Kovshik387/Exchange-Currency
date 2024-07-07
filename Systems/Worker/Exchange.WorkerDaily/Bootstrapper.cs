using Exchange.Services.Logger;
using Exchange.Services.RabbitMq;

namespace Exchange.WorkerDaily;

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
