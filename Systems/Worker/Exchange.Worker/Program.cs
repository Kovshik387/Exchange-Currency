using Exchange.Common.Settings;
using Exchange.Services.Logger.Logger;
using Exchange.Services.Settings.SettingsConfigure;
using Exchange.Worker;
using Exchange.Worker.Configuration;
using Exchange.Worker.TaskExecutor;

var builder = WebApplication.CreateBuilder(args);

var logSettings = Settings.Load<LogSettings>("Log");
builder.AddAppLogger(logSettings);

builder.Services.AddHttpContextAccessor();

builder.Services.RegisterAppServices();

builder.Services.AddHostedService<TaskExecutor>();

var app = builder.Build();

var logger = app.Services.GetService<IAppLogger>();

logger.Information("Worker has started");

app.Run();

logger.Information("Worker has stopped");