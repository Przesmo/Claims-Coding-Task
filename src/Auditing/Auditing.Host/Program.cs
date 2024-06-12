using Auditing.Host;
using Auditing.Host.MessagesHandler;
using Auditing.Host.Repositories;
using EasyNetQ.Consumer;

var builder = Host.CreateApplicationBuilder(args);
//ToDo: move to appsettings
var connectionString = "host=localhost;username=admin;password=admin";
builder.Services
    .RegisterAuditLogsRepository(builder.Configuration)
    .RegisterEasyNetQ(connectionString)
    .AddSingleton<IAddAuditLogHandler, AddAuditLogHandler>()
    .AddSingleton<IConsumer, Consumer>()
    .AddHostedService<ConsumerSubscriptionService>();

var host = builder.Build();
host.Run();
