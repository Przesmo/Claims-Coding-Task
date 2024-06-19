using Insurance.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Insurance.Application;

public static class ApplicationRegistration
{
    public static IServiceCollection RegisterApplication(this IServiceCollection services)
    {
        return services.AddTransient<IClaimsService, ClaimsService>()
            .AddTransient<ICoversService, CoversService>();
    }
}
