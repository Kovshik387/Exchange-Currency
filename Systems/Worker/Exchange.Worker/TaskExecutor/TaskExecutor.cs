using Exchange.Services.EmailAction.Data;
using Exchange.Services.Logger.Logger;
using Exchange.Services.RabbitMq.Infrastructure;
using System.Text;
using System.Threading.Channels;

namespace Exchange.Worker.TaskExecutor;

public class TaskExecutor : BackgroundService
{
    private readonly IAppLogger logger;
    private readonly IRabbitMq rabbitMq;
    public TaskExecutor(
        IAppLogger logger,
        IRabbitMq rabbitMq
    )
    {
        this.logger = logger;
        this.rabbitMq = rabbitMq;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            rabbitMq.Subscribe<EmailModel>(RabbitQueue.PUBLICATE_ACCOUNT, async data =>
            {
                logger.Information($"email send: {data.accountModel.Email}");
                foreach(var item in data.voluteModel)
                {
                    logger.Information($"{item.Name}: {item.Value}");
                }
            });
        }
    }
}
