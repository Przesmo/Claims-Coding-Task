using Auditing.Host.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Auditing.ComponentTests.Configuration;

public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);
        builder.ConfigureTestServices(services =>
        {
            var serviceProvider = services.BuildServiceProvider();
            services.AddMemoryCache();
            var descriptor = new ServiceDescriptor(
                typeof(IAuditLogRepository),
                typeof(RepositoryTestDouble),
                ServiceLifetime.Singleton);
            services.Replace(descriptor);
            services.AddSingleton<RepositoryTestDouble>();
        });
        builder.UseEnvironment("test");
    }

    public static WebApplicationFactory<TProgram> Instance { get; } = new CustomWebApplicationFactory<TProgram>()
        .WithWebHostBuilder(builder =>
        {
            builder.Configure(_ => { });
        });
}
