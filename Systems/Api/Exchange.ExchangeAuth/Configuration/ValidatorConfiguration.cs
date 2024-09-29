using Exchange.Common.Helpers;
using Exchange.Common.Validator;
using FluentValidation.AspNetCore;

namespace Exchange.ExchangeAuth.Configuration;

public static class ValidatorConfiguration
{
    public static IServiceCollection AddAppValidator(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation(opt => { opt.DisableDataAnnotationsValidation = true; });

        ValidatorsRegisterHelper.Register(services);

        services.AddSingleton(typeof(IModelValidator<>), typeof(ModelValidator<>));

        return services;
    }
}