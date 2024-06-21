using Auditing.ComponentTests.TestDoubles;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace Auditing.ComponentTests.Configuration;

public class CustomWebApplicationFactory<TProgram> :
    WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);
        builder.ConfigureTestServices(services =>
        {
            services.AddSingleton<AuditTestRepository>();
        });
        builder.UseEnvironment("test");
    }

    public static WebApplicationFactory<TProgram> Instance { get; } =
        new CustomWebApplicationFactory<TProgram>()
        .WithWebHostBuilder(builder =>
        {
            builder.Configure(_ => { });
        });
}
