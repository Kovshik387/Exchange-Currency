using Exchange.Services.Settings.SettingsConfigure;
using Exchange.Common.Settings;
using Exchange.ExchangeAccount.Configuration;
using Exchange.ExchangeAccount;
using Exchange.Account.Context.Setup;
using Exchange.Account.Context;

var builder = WebApplication.CreateBuilder(args);

var mainSettings = Settings.Load<MainSettings>("Main");
var logSettings = Settings.Load<LogSettings>("Log");
var swaggerSettings = Settings.Load<SwaggerSettings>("Swagger");
var authSettings = Settings.Load<AuthSettings>("Auth");

builder.AddAppLogger(mainSettings, logSettings);

builder.Services.AddAppDbContext(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAppAutoMappers();
builder.Services.AddAppSwagger(mainSettings, swaggerSettings, authSettings);
builder.Services.RegisterService();
builder.Services.AddAppValidator();
builder.Services.AddAppCors();

var app = builder.Build();

app.UseAppSwagger();
app.UseAppCors();

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

DbInitializer.Execute(app.Services);

app.Run();
