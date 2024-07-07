namespace Exchange.Services.Settings.SettingsConfigure;

public class RabbitMqSettings
{
    public string Uri { get; private set; } = null!;
    public string UserName { get; private set; } = null!;
    public string Password { get; private set; } = null!;
}
