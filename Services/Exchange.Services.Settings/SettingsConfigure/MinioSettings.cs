namespace Exchange.Services.Settings.SettingsConfigure;

public class MinioSettings
{
    /// <summary>
    /// Подключение к Minio
    /// </summary>
    public string EndPoint { get; set; } = string.Empty;
    /// <summary>
    /// Имя пользователя
    /// </summary>
    public string AccessKey {  get; set; } = string.Empty;
    /// <summary>
    /// Секрет 
    /// </summary>
    public string SecretKey { get; set; } = string.Empty;
    /// <summary>
    /// Включение SSL подключения
    /// </summary>
    public bool Ssl {  get; set; } = false;
}