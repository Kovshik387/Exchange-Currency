using Exchange.Services.RabbitMq.Infrastructure;
using Exchange.Services.Settings.SettingsConfigure;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System.Text;
using System.Text.Json;

namespace Exchange.Services.RabbitMq.RabbitMq;

public class RabbitMq : IRabbitMq, IDisposable
{
    const int connectRetriesCount = 10;

    private readonly object _lock = new object();
    private readonly RabbitMqSettings _rabbitMqSettings;
    private IModel channel;

    private IConnection connection;

    public RabbitMq(RabbitMqSettings settings) => _rabbitMqSettings = settings;

    public void Dispose()
    {
        channel?.Close();
        connection?.Close();
    }

    private IModel GetChannel()
    {
        return channel;
    }

    private async Task RegisterListener(string queueName, EventHandler<BasicDeliverEventArgs> onReceive)
    {
        Connect();

        AddQueue(queueName);

        var consumer = new EventingBasicConsumer(channel);

        consumer.Received += onReceive;

        channel.BasicConsume(queueName, false, consumer);
    }

    private async Task Publish<T>(string queueName, T data)
    {
        Connect();

        AddQueue(queueName);

        var json = JsonSerializer.Serialize<object>(data, new JsonSerializerOptions() { });

        var message = Encoding.UTF8.GetBytes(json);

        channel.BasicPublish(string.Empty, queueName, null, message);
    }

    private void Connect()
    {
        lock (_lock)
        {
            if (connection?.IsOpen ?? false)
                return;

            var factory = new ConnectionFactory
            {
                Uri = new Uri(_rabbitMqSettings.Uri),
                UserName = _rabbitMqSettings.UserName,
                Password = _rabbitMqSettings.Password,

                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(5)
            };

            var retriesCount = 0;
            while (retriesCount < connectRetriesCount)
                try
                {
                    if (connection == null)
                    {
                        connection = factory.CreateConnection();
                        Console.WriteLine("connection");
                    }

                    if (channel == null)
                    {
                        channel = connection.CreateModel();
                        channel.BasicQos(0, 1, false);
                    }

                    break;
                }
                catch (BrokerUnreachableException)
                {
                    Task.Delay(500).Wait();

                    retriesCount++;
                }
        }
    }

    private void AddQueue(string queueName)
    {
        Connect();
        channel.QueueDeclare(queueName, true, false, false, null);
    }

    public async Task Subscribe<T>(string queueName, OnDataReceiveEvent<T> onReceive)
    {
        if (onReceive == null)
            return;

        await RegisterListener(queueName, async (_, eventArgs) =>
        {
            var channel = GetChannel();
            try
            {
                var message = Encoding.UTF8.GetString(eventArgs.Body.ToArray());

                var obj = JsonSerializer.Deserialize<T>(message ?? "");

                await onReceive(obj);
                channel.BasicAck(eventArgs.DeliveryTag, false);
            }
            catch (Exception e)
            {
                channel.BasicNack(eventArgs.DeliveryTag, false, false);
            }
        });
    }

    public async Task PushAsync<T>(string queueName, T data)
    {
        await Publish(queueName, data);
    }
}
