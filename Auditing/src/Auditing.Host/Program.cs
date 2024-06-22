using Auditing.Host;
using Auditing.Host.HealthChekcs;
using Auditing.Host.MessagesConsumer;
using Auditing.Host.MessagesHandler;
using Auditing.Host.Metrics;
using Auditing.Host.Repositories;
using Auditing.Infrastructure;
using EasyNetQ.Consumer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true)
    .AddEnvironmentVariables()
    .Build();

builder.Services
    .RegisterAuditLogsRepository(builder.Configuration)
    .RegisterEasyNetQ(builder.Configuration.GetSection("Bus:ConnectionString").Value)
    .AddSingleton<IAddAuditLogHandler, AddAuditLogHandler>()
    .Decorate<IAddAuditLogHandler, LoggingHandler>()
    .AddSingleton<IConsumer, CustomConsumer>()
    .ConfigureHealthChecks(builder.Configuration)
    .ConfigureMetrics()
    .AddHostedService<ConsumerSubscriptionService>();

var app = builder.Build();
app.UseOpenTelemetryPrometheusScrapingEndpoint();

using var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<AuditContext>();
context.Database.Migrate();

app.Run();

public partial class Program { }