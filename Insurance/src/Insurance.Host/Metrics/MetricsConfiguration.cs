using OpenTelemetry.Metrics;

namespace Insurance.Host.Metrics;

public static class MetricsConfiguration
{
    public static IServiceCollection ConfigureMetrics(this IServiceCollection services)
    {
        services.AddOpenTelemetry()
            .WithMetrics(builder =>
            {
                builder
                    .AddPrometheusExporter()
                    .AddRuntimeInstrumentation()
                    .AddProcessInstrumentation()
                    .AddAspNetCoreInstrumentation()
                    .AddMeter(
                        "Microsoft.AspNetCore.Hosting",
                        "Microsoft-AspNetCore-Server-Kestrel",
                        "System.Net.Sockets",
                        "System.Net.Security");
            });
        return services;
    }
}
