using Auditing.Host;
using Auditing.Host.MessagesHandler;
using EasyNetQ.Consumer;

var builder = Host.CreateApplicationBuilder(args);
var connectionString = "host=localhost;username=admin;password=admin";
builder.Services
    .RegisterEasyNetQ(connectionString)
    .AddSingleton<IAddAuditLogHandler, AddAuditLogHandler>()
    .AddSingleton<IConsumer, Consumer>()
    .AddHostedService<ConsumerSubscriptionService>();

var host = builder.Build();
host.Run();
