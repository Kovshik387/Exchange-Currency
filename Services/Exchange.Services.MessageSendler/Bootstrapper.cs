using Exchange.Services.MessageSendler.Infrastructure;
using Exchange.Services.MessageSendler.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Exchange.Services.MessageSendler;

public static class Bootstrapper
{
    public static IServiceCollection AddMessageService(this IServiceCollection services)
    {
        return services.AddTransient<IMessageSendler, Services.MessageSendler>();
    }
}