using Exchange.Common.Settings;
using Exchange.ExchangeVolute;
using Exchange.ExchangeVolute.Configuration;
using Exchange.Services.Settings.SettingsConfigure;
using Exchange.Context;
using Exchange.Context.Setup;
using Exchange.ExchangeValute.Configuration;
using Exchange.ExchangeValute.Services;

var builder = WebApplication.CreateBuilder(args);

var mainSettings = Settings.Load<MainSettings>("Main");
var logSettings = Settings.Load<LogSettings>("Log");
var swaggerSettings = Settings.Load<SwaggerSettings>("Swagger");

builder.AddAppLogger(mainSettings, logSettings);

builder.Services.AddAppDbContext(builder.Configuration);
builder.Services.AddGrpc();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.RegisterServices();
builder.Services.AddAppAutoMappers();
builder.Services.AddAppSwagger(swaggerSettings);
builder.Services.AddAppCors();
var app = builder.Build();

app.UseAppSwagger();

app.MapGrpcService<ValuteService>();
app.UseAppCors();
app.UseHttpsRedirection();
app.MapControllers();

DbInitializer.Execute(app.Services);

app.Run();
