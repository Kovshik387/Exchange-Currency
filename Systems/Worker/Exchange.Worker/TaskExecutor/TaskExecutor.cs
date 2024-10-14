using Exchange.Services.EmailAction.Data;
using Exchange.Services.MessageSendler.Infrastructure;
using Exchange.Services.RabbitMq.Infrastructure;

namespace Exchange.Worker.TaskExecutor;

public class TaskExecutor : BackgroundService
{
    private readonly IRabbitMq _rabbitMq;
    private readonly IMessageSendler _messageHandler;
    public TaskExecutor(IRabbitMq rabbitMq, IMessageSendler messageHandler)
    {
        this._rabbitMq = rabbitMq;
        this._messageHandler = messageHandler;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _rabbitMq.Subscribe<EmailModel>(RabbitQueue.PUBLICATE_ACCOUNT, _messageHandler.SendNotificationAsync);
        while (!stoppingToken.IsCancellationRequested) { await Task.Delay(100, stoppingToken); }
    }
}
