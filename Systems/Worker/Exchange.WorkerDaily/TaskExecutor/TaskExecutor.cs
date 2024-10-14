using Exchange.Services.EmailAction.Data;
using Exchange.Services.Logger.Logger;
using Exchange.Services.RabbitMq.Infrastructure;
using Exchange.Services.Settings.SettingsConfigure;
using Newtonsoft.Json;

namespace Exchange.WorkerDaily.TaskExecutor;

public class TaskExecutor : BackgroundService
{
    private readonly IAppLogger _logger;
    private readonly IRabbitMq _rabbitMq;
    private readonly HttpClient _httpClient;
    private readonly ApiKeySettings _settings;
    private static readonly Uri AccountUri = new Uri("http://host.docker.internal:8030/api/accounts");
    private const string VoluteUrl = "http://host.docker.internal:8020/api/exchange/exchange-rates?date=";

    public TaskExecutor(IAppLogger logger, IRabbitMq rabbitMq, ApiKeySettings settings)
    {
        _logger = logger;
        _rabbitMq = rabbitMq;
        _settings = settings;
        _httpClient = new HttpClient();
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.Information(DateTime.UtcNow.Hour.ToString());
            if (DateTime.UtcNow.Hour + 3 == 18)
            {
                _logger.Information($"time: {DateOnly.FromDateTime(DateTime.Now)}");
                var uri = new Uri(VoluteUrl+DateOnly.FromDateTime(DateTime.Now).AddDays(1));
                var responseVolute = await _httpClient.GetAsync(uri, stoppingToken);
                if (!responseVolute.IsSuccessStatusCode)
                {
                    _logger.Fatal("Api Volute not working");
                }

                var volutes = JsonConvert.DeserializeObject<ExchangeModel>(await responseVolute.Content.ReadAsStringAsync(stoppingToken));

                if (DateOnly.Parse(volutes!.date) < DateOnly.FromDateTime(DateTime.UtcNow))
                {
                    _logger.Information($"data from response: {volutes.date} : data now");
                    return;
                }

                var requestAccount = new HttpRequestMessage(HttpMethod.Get, AccountUri);
                requestAccount.Headers.Add("secret", _settings.XAPIKEY);

                var responseAccount = await _httpClient.SendAsync(requestAccount, stoppingToken);
                if (!responseAccount.IsSuccessStatusCode)
                {
                    _logger.Fatal("Api Account is not working");
                }

                var accounts = JsonConvert.DeserializeObject<List<AccountModel>>(await responseAccount.Content.ReadAsStringAsync(stoppingToken));

                if (accounts != null)
                    foreach (var item in accounts)
                    {
                        await _rabbitMq.PushAsync(RabbitQueue.PUBLICATE_ACCOUNT, new EmailModel()
                        {
                            AccountModel = new AccountModel()
                            {
                                Email = item.Email,
                                Name = item.Name,
                            },
                            Date = volutes.date,
                            VoluteModel = volutes.Volute
                        });
                    }
            }
            await Task.Delay(1000 * 60 * 60, stoppingToken);
        }
    }
}
