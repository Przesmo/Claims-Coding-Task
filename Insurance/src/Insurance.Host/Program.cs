using Insurance.Application;
using Insurance.Host.ExceptionHandlers;
using Insurance.Host.HealthChecks;
using Insurance.Host.Swagger;
using Insurance.Infrastructure.AuditingIntegration;
using Insurance.Infrastructure.Repositories;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);
builder.Configuration.SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true)
    .AddEnvironmentVariables()
    .Build();

builder.Services
    .RegisterRepositories(builder.Configuration)
    .RegisterAuditingIntegration(builder.Configuration)
    .RegisterApplication()
    .AddControllers()
    .AddJsonOptions(x =>
    {
        x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services
    .AddApiVersioning(options =>
    {
        options.ReportApiVersions = true;
    })
    .AddApiExplorer(
    options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });

builder.Services
    .AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>()
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .ConfigureHealthChecks(builder.Configuration)
    .AddExceptionHandler<ClaimNotCoveredExceptionHandler>()
    .AddExceptionHandler<InsurancePeriodExceededExceptionHandler>()
    .AddExceptionHandler<GlobalExceptionHandler>()
    .AddProblemDetails();

var app = builder.Build();

app.UseExceptionHandler()
   .UseSwagger()
   .UseSwaggerUI(options =>
   {
       var descriptions = app.DescribeApiVersions();
       foreach (var description in descriptions)
       {
           options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
       }
   })
   .UseHttpsRedirection()
   .UseAuthorization();
app.MapControllers();

app.Run();

public partial class Program { }
