using Auditing.Host;
using Auditing.Host.MessagesConsumer;
using Auditing.Host.MessagesHandler;
using Auditing.Host.Repositories;
using Auditing.Infrastructure;
using EasyNetQ.Consumer;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);
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
    .AddHostedService<ConsumerSubscriptionService>();

var host = builder.Build();

using var scope = host.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<AuditContext>();
context.Database.Migrate();

host.Run();

public partial class Program { }