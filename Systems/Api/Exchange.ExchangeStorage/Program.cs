using Exchange.Common.Settings;
using Exchange.ExchangeStorage;
using Exchange.ExchangeStorage.Configuration;
using Exchange.ExchangeStorage.Services;
using Exchange.Services.Settings.SettingsConfigure;

var builder = WebApplication.CreateBuilder(args);
var mainSettings = Settings.Load<MainSettings>("Main");
var minioSettings = Settings.Load<MinioSettings>("Minio");
var logSettings = Settings.Load<LogSettings>("Log");
var swaggerSettings = Settings.Load<SwaggerSettings>("Swagger");

builder.AddAppLogger(mainSettings, logSettings);
builder.Services.AddControllers();
builder.Services.AddGrpc();
builder.Services.AddAppSwagger(mainSettings,swaggerSettings);
builder.Services.AddAppMinio(minioSettings);

builder.Services.RegisterServices();

var app = builder.Build();
app.UseAppSwagger();
app.UseAppCors();

app.UseHttpsRedirection();
app.MapGrpcService<StorageService>();

app.Run();
