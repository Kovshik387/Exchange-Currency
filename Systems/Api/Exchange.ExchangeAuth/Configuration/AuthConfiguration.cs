using System.Net;
using Exchange.Services.Settings.SettingsConfigure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Exchange.ExchangeAuth.Configuration;

public static class AuthConfiguration
{
    public static IServiceCollection AddAppAuth(this IServiceCollection services, AuthSettings settings)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = settings.Issuer,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = settings.SymmetricSecurityKeyAccess,
                    ValidateAudience = true,
                    ValidAudience = settings.Audience,
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                };
                options.Events = new JwtBearerEvents
                {
                    OnChallenge = context =>
                    {
                        context.HandleResponse();

                        context.Response.ContentType = "application/json";
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        return context.Response.WriteAsync("{\"error\":\"Unauthorized access\"}");
                    }
                };
            });

        return services;
    }

    public static IApplicationBuilder UseAppAuth(this IApplicationBuilder app)
    {
        app.UseAuthentication();

        app.UseAuthorization();

        return app;
    }
}