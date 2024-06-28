using Exchange.Common.Settings;
using Exchange.ExchangeCache;
using Exchange.ExchangeCache.Configuration;
using Exchange.Services.Settings.SettingsConfigure;

var builder = WebApplication.CreateBuilder(args);

var mainSettings = Settings.Load<MainSettings>("Main");
var logSettings = Settings.Load<LogSettings>("Log");
var swaggerSettings = Settings.Load<SwaggerSettings>("Swagger");
var redisSettings = Settings.Load<RedisSettings>("Redis");
builder.AddAppLogger(mainSettings, logSettings);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAppSwagger(swaggerSettings);
builder.Services.RegisterServices();
builder.Services.AddAppCors();
builder.Services.AddAppRedis(redisSettings);

var app = builder.Build();

app.UseAppSwagger();
app.UseAppCors();

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
