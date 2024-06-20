using Insurance.Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Insurance.Host.ExceptionHandlers;

internal sealed class ClaimNotCoveredExceptionHandler : IExceptionHandler
{
    private readonly ILogger<ClaimNotCoveredExceptionHandler> _logger;

    public ClaimNotCoveredExceptionHandler(ILogger<ClaimNotCoveredExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not ClaimNotCoveredException claimNotCoveredException)
        {
            return false;
        }

        _logger.LogError(
            claimNotCoveredException,
            "Exception occurred: {Message}",
            claimNotCoveredException.Message);

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Bad Request",
            Detail = claimNotCoveredException.Message
        };

        httpContext.Response.StatusCode = problemDetails.Status.Value;

        await httpContext.Response
            .WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}
