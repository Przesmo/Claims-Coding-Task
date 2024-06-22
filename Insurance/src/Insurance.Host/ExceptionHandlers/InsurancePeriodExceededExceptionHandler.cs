using Insurance.Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Insurance.Host.ExceptionHandlers;

public class InsurancePeriodExceededExceptionHandler : IExceptionHandler
{
    private readonly ILogger<InsurancePeriodExceededExceptionHandler> _logger;

    public InsurancePeriodExceededExceptionHandler(
        ILogger<InsurancePeriodExceededExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not InsurancePeriodExceededException insurancePeriodExceededException)
        {
            return false;
        }

        _logger.LogError(
            insurancePeriodExceededException,
            "Exception occurred: {Message}",
            insurancePeriodExceededException.Message);

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Bad Request",
            Detail = insurancePeriodExceededException.Message
        };

        httpContext.Response.StatusCode = problemDetails.Status.Value;

        await httpContext.Response
            .WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}

