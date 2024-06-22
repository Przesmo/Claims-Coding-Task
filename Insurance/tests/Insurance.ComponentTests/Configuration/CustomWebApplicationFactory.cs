using Insurance.ComponentTests.TestDoubles;
using Insurance.Infrastructure.AuditingIntegration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Insurance.ComponentTests.Configuration;

internal class CustomWebApplicationFactory<TProgram> :
    WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);
        builder.ConfigureTestServices(services =>
        {
            var serviceProvider = services.BuildServiceProvider();
            var descriptor = new ServiceDescriptor(
                typeof(IAuditingQueue),
                typeof(AuditingQueueTestDouble),
                ServiceLifetime.Singleton);
            services.Replace(descriptor);
        });
        builder.UseEnvironment("test");
    }

    public static WebApplicationFactory<TProgram> Instance { get; } =
        new CustomWebApplicationFactory<TProgram>();
}
