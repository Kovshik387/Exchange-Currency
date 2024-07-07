using Exchange.Services.EmailAction.Data;
using Exchange.Services.Logger.Logger;
using Exchange.Services.RabbitMq.Infrastructure;
using Newtonsoft.Json;

namespace Exchange.WorkerDaily.TaskExecutor;

public class TaskExecutor : BackgroundService
{
    private readonly IAppLogger _logger;
    private readonly IRabbitMq _rabbitMq;
    private readonly HttpClient _httpClient;
    private static Uri _accountUri = new Uri("http://host.docker.internal:8030/api/accounts");
    public TaskExecutor(IAppLogger logger, IRabbitMq rabbitMq)
    {
        _logger = logger;
        _rabbitMq = rabbitMq;
        _httpClient = new HttpClient();
    }
    const string url = "http://host.docker.internal:8020/exchange-rates?date=07.07.2024";
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var uri = new Uri(url);
            var responseVolute = await _httpClient.GetAsync(uri);
            if (!responseVolute.IsSuccessStatusCode)
            {
                _logger.Fatal("Api Volute not working");
            }

            var volutes = JsonConvert.DeserializeObject<ExchangeModel>(await responseVolute.Content.ReadAsStringAsync());

            var responseAccount = await _httpClient.GetAsync(_accountUri);
            if (!responseAccount.IsSuccessStatusCode)
            {
                _logger.Fatal("Api Account is not working");
            }

            var accounts = JsonConvert.DeserializeObject<List<AccountModel>>(await responseAccount.Content.ReadAsStringAsync());

            foreach(var item in accounts)
            {
                await _rabbitMq.PushAsync(RabbitQueue.PUBLICATE_ACCOUNT, new EmailModel()
                {
                    accountModel = new AccountModel()
                    {
                        Email = item.Email,
                        Name = item.Name,
                    },
                    voluteModel = volutes.Volute
                });
            }

            await Task.Delay(1000 * 100);
            //await Task.Delay(1000 * 60 * 5);
        }
    }

}
