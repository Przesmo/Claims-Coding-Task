using Auditing.Host;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<ConsumerSubscriptionService>();

var host = builder.Build();
host.Run();
