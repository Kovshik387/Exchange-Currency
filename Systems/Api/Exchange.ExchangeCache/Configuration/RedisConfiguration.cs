using Exchange.Services.Settings.SettingsConfigure;

namespace Exchange.ExchangeCache.Configuration
{
    public static class RedisConfiguration
    {
        public static IServiceCollection AddAppRedis(this IServiceCollection services, RedisSettings redisSettings)
        {
            Console.WriteLine($"url: {redisSettings.Configuration} instanse: {redisSettings.InstanseName}");
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisSettings.Configuration;
                options.InstanceName = redisSettings.InstanseName;
            });

            return services;
        }
    }
}
