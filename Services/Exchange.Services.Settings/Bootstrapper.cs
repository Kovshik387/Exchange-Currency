﻿using Exchange.Services.Settings.SettingsConfigure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Exchange.Services.Settings;
public static class Bootstrapper
{
    public static IServiceCollection AddMainSettings(this IServiceCollection services, IConfiguration configuration = null)
    {
        var settings = Common.Settings.Settings.Load<MainSettings>("Main", configuration);
        services.AddSingleton(settings);

        return services;
    }

    public static IServiceCollection AddSwaggerSettings(this IServiceCollection services, IConfiguration configuration = null)
    {
        var settings = Common.Settings.Settings.Load<SwaggerSettings>("Swagger", configuration);
        services.AddSingleton(settings);

        return services;
    }

    public static IServiceCollection AddLogSettings(this IServiceCollection services, IConfiguration configuration = null)
    {
        var settings = Common.Settings.Settings.Load<LogSettings>("Log", configuration);
        services.AddSingleton(settings);

        return services;
    }

    public static IServiceCollection AddReddisSettings(this IServiceCollection services, IConfiguration configuration = null)
    {
        var settings = Common.Settings.Settings.Load<RedisSettings>("Redis", configuration);
        services.AddSingleton(settings);

        return services;
    }
    public static IServiceCollection AddApiPathsSettings(this IServiceCollection services, IConfiguration configuration = null)
    {
        var settings = Common.Settings.Settings.Load<ApiEndPointSettings>("ApiEndPointSettings", configuration);
        services.AddSingleton(settings);

        return services;
    }
    public static IServiceCollection AddAuthSettings(this IServiceCollection services, IConfiguration configuration = null)
    {
        var settings = Common.Settings.Settings.Load<AuthSettings>("Auth", configuration);
        services.AddSingleton(settings);

        return services;
    }

    public static IServiceCollection AddEmailSettings(this IServiceCollection services, IConfiguration configuration = null)
    {
        var settings = Common.Settings.Settings.Load<EmailSettings>("Email", configuration);
        services.AddSingleton(settings);

        return services;
    }

    public static IServiceCollection AddApiKeySettings(this IServiceCollection services, IConfiguration configuration = null)
    {
        var settings = Common.Settings.Settings.Load<ApiKeySettings>("ApiKey", configuration);
        services.AddSingleton(settings);

        return services;
    }
    
    public static IServiceCollection AddMinioSettings(this IServiceCollection services, IConfiguration configuration = null)
    {
        var settings = Common.Settings.Settings.Load<MinioSettings>("Minio", configuration);
        services.AddSingleton(settings);

        return services;
    }
}