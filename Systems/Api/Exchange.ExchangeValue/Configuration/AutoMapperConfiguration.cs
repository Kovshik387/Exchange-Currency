using Exchange.Common.Helpers;

namespace Exchange.ExchangeValute.Configuration;

public static class AutoMapperConfiguration
{
    public static IServiceCollection AddAppAutoMappers(this IServiceCollection services)
    {
        AutoMappersRegisterHelper.Register(services);

        return services;
    }
}
