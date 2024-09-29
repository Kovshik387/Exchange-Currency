using System.Reflection;
using Exchange.Services.Settings.SettingsConfigure;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Exchange.ExchangeAuth.Configuration;

public static class SwaggerConfiguration
{
    private static string AppTitle = "AccountApi";

    /// <summary>
    /// Add OpenAPI to API
    /// </summary>
    /// <param name="services">Services collection</param>
    /// <param name="mainSettings"></param>
    /// <param name="swaggerSettings"></param>
    public static IServiceCollection AddAppSwagger(this IServiceCollection services,
        MainSettings mainSettings,
        SwaggerSettings swaggerSettings,
        AuthSettings identitySettings
        )
    {
        if (!swaggerSettings.Enabled)
            return services;

        services
            .AddOptions<SwaggerGenOptions>();

        services.AddSwaggerGen(options =>
        {
            options.SupportNonNullableReferenceTypes();

            options.UseInlineDefinitionsForEnums();

            options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

            options.DescribeAllParametersInCamelCase();

            options.CustomSchemaIds(x => x.FullName);

            var xmlPath = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
            if (File.Exists(xmlPath))
                options.IncludeXmlComments(xmlPath);

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please insert JWT with Bearer into field",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new List<string>()
                }
            });

            options.UseOneOfForPolymorphism();
            options.EnableAnnotations(true, true);

            options.UseAllOfForInheritance();
            options.UseOneOfForPolymorphism();

            options.SelectSubTypesUsing(baseType =>
                typeof(Program).Assembly.GetTypes().Where(type => type.IsSubclassOf(baseType))
            );

            options.ExampleFilters();
        });

        services.AddSwaggerExamplesFromAssemblies(Assembly.GetEntryAssembly());

        services.AddSwaggerGenNewtonsoftSupport();

        return services;
    }


    /// <summary>
    /// Start OpenAPI UI
    /// </summary>
    /// <param name="app">Web application</param>
    public static void UseAppSwagger(this WebApplication app)
    {
        var mainSettings = app.Services.GetService<MainSettings>();
        var swaggerSettings = app.Services.GetService<SwaggerSettings>();

        if (!swaggerSettings?.Enabled ?? false)
            return;

        //var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

        app.UseSwagger(/*options => { options.RouteTemplate = "/docs/{documentname}/api.json"; }*/);

        app.UseSwaggerUI(
            options =>
            {
                //options.RoutePrefix = "docs";
                //provider.ApiVersionDescriptions.ToList().ForEach(
                //    description =>
                //        options.SwaggerEndpoint(
                //            mainSettings!.PublicUrl + $"/docs/{description.GroupName}/api.json",
                //            description.GroupName.ToUpperInvariant())
                //);
                options.DocExpansion(DocExpansion.List);
                options.DefaultModelsExpandDepth(-1);
                options.OAuthAppName(AppTitle);

                //options.OAuthClientId(swaggerSettings?.OAuthClientId ?? "");
                //options.OAuthClientSecret(swaggerSettings?.OAuthClientSecret ?? "");
            }


        );
    }
}