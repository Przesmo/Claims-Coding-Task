using Auditing.Host;
using Auditing.Host.MessagesConsumer;
using Auditing.Host.MessagesHandler;
using Auditing.Host.Repositories;
using Auditing.Infrastructure;
using EasyNetQ.Consumer;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);

builder.Services
    .RegisterAuditLogsRepository(builder.Configuration)
    .RegisterEasyNetQ(builder.Configuration.GetSection("Bus:ConnectionString").Value)
    .AddSingleton<IAddAuditLogHandler, AddAuditLogHandler>()
    .Decorate<IAddAuditLogHandler, LoggingHandler>()
    .AddSingleton<IConsumer, CustomConsumer>()
    .AddHostedService<ConsumerSubscriptionService>();

var host = builder.Build();

if (builder.Environment.EnvironmentName != "test")
{
    using (var scope = host.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<AuditContext>();
        context.Database.Migrate();
    }
}

host.Run();

public partial class Program { }