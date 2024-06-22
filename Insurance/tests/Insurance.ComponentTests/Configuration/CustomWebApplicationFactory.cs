using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Insurance.ComponentTests.Configuration;

internal class CustomWebApplicationFactory<TProgram> :
    WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);
        builder.UseEnvironment("test");
    }

    public static WebApplicationFactory<TProgram> Instance { get; } =
        new CustomWebApplicationFactory<TProgram>();
}
