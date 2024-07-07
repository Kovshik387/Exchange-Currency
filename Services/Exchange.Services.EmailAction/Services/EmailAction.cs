using Exchange.Services.EmailAction.Data;
using Exchange.Services.EmailAction.Infrastructure;
using Exchange.Services.RabbitMq.Infrastructure;

namespace Exchange.Services.EmailAction.Services;

public class EmailAction : IEmailAction
{
    private readonly IRabbitMq rabbitMq;

    public EmailAction(IRabbitMq rabbitMq)
    {
        this.rabbitMq = rabbitMq;
    }

    public async Task PublicateAccount(EmailModel model)
    {
        await rabbitMq.PushAsync(RabbitQueue.PUBLICATE_ACCOUNT, model);
    }
}
