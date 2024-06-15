using Auditing.Host;
using Auditing.Host.MessagesConsumer;
using Auditing.Host.MessagesHandler;
using Auditing.Host.Repositories;
using Auditing.Infrastructure;
using EasyNetQ.Consumer;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);
//ToDo: move to appsettings
var connectionString = "host=localhost;username=guest;password=guest";
builder.Services
    .RegisterAuditLogsRepository(builder.Configuration)
    .RegisterEasyNetQ(connectionString)
    .AddSingleton<IAddAuditLogHandler, AddAuditLogHandler>()
    .AddSingleton<IConsumer, CustomConsumer>()
    .AddHostedService<ConsumerSubscriptionService>();

var host = builder.Build();

using (var scope = host.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AuditContext>();
    context.Database.Migrate();
}

host.Run();
