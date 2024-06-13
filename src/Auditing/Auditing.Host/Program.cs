using Auditing.Host;
using Auditing.Host.MessagesConsumer;
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
    .AddSingleton<IConsumer, CustomConsumer>()
    .AddHostedService<ConsumerSubscriptionService>();

//ToDo: Chceck if it is needed and why
/*using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AuditContext>();
    context.Database.Migrate();
}*/

var host = builder.Build();
host.Run();
