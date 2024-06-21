using Exchange.Common.Settings;
using Exchange.ExchangeValute;
using Exchange.ExchangeValute.Configuration;
using Exchange.Services.Settings.SettingsConfigure;

var builder = WebApplication.CreateBuilder(args);

var mainSettings = Settings.Load<MainSettings>("Main");
var logSettings = Settings.Load<LogSettings>("Log");

builder.AddAppLogger(mainSettings, logSettings);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen().AddCors();
builder.Services.RegisterServices();


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
