using Exchange.Authorization.Context;
using Exchange.Authorization.Context.Setup;
using Exchange.ExchangeAuth.Configuration;
using Exchange.Common.Settings;
using Exchange.Services.Settings.SettingsConfigure;
using Exchange.ExchangeAuth;
using Exchange.Services.Authorization;

var builder = WebApplication.CreateBuilder(args);

var mainSettings = Settings.Load<MainSettings>("Main");
var logSettings = Settings.Load<LogSettings>("Log");
var swaggerSettings = Settings.Load<SwaggerSettings>("Swagger");
var authSettings = Settings.Load<AuthSettings>("Auth");

builder.AddAppLogger(mainSettings, logSettings);

builder.Services.AddAppCors();
builder.Services.AddAppDbContext(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddGrpc();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAppAuth(authSettings);
builder.Services.AddAppSwagger(mainSettings, swaggerSettings, authSettings);
builder.Services.AddAppAutoMappers();
builder.Services.AddAppValidator();
builder.Services.AddAuthService();

builder.Services.RegisterService(builder.Configuration);

var app = builder.Build();

app.UseAppCors();
app.UseAppSwagger();
app.UseAppAuth();

app.UseHttpsRedirection();

app.MapControllers();

DbInitializer.Execute(app.Services);

app.Run();