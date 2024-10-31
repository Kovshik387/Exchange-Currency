using System.Net;
using Exchange.Services.Settings.SettingsConfigure;
using Minio;

namespace Exchange.ExchangeStorage.Configuration;

public static class MinioConfiguration
{
    public static IServiceCollection AddAppMinio(this IServiceCollection services, MinioSettings minioSettings)
    {
        services.AddMinio(x => x
            .WithEndpoint(minioSettings.EndPoint)
            .WithProxy(new WebProxy("minio",9000))
            .WithCredentials(minioSettings.AccessKey,minioSettings.SecretKey)
            .WithSSL(minioSettings.Ssl)
        );
        return services;
    }
}