using Exchange.Authorization.Context;
using Exchange.Authorization.Context.Setup;
using Exchange.ExchangeAuth.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAppDbContext(builder.Configuration);

var app = builder.Build();
app.UseAppCors();
app.UseAppSwagger();
app.UseAppAuth();

app.UseHttpsRedirection();

app.MapControllers();

DbInitializer.Execute(app.Services);

app.Run();
