using Exchange.Services.EmailAction.Data;
using Exchange.Services.Logger.Logger;
using Exchange.Services.MessageSendler.Infrastructure;
using Exchange.Services.RabbitMq.Infrastructure;
using System.Text;
using System.Threading.Channels;

namespace Exchange.Worker.TaskExecutor;

public class TaskExecutor : BackgroundService
{
    private readonly IAppLogger logger;
    private readonly IRabbitMq rabbitMq;
    private readonly IMessageSendler _messageSendler;
    public TaskExecutor(IAppLogger logger, IRabbitMq rabbitMq, IMessageSendler messageSendler)
    {
        this.logger = logger;
        this.rabbitMq = rabbitMq;
        this._messageSendler = messageSendler;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await rabbitMq.Subscribe<EmailModel>(RabbitQueue.PUBLICATE_ACCOUNT, _messageSendler.SendNotificationAsync);
        while (!stoppingToken.IsCancellationRequested) { await Task.Delay(100); }
    }
}
