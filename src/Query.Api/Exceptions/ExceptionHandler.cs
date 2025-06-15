using Assessments.ExpenseManagement.Domain.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Assessments.ExpenseManagement.Query.Api.Exceptions;

public sealed class ExceptionHandler(IProblemDetailsService problemDetailsService) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken) =>
        exception switch
        {
            DomainException domainException => await TryHandleAsync(httpContext, domainException, cancellationToken),
            ValidationException validationException =>
                await TryHandleAsync(httpContext, validationException, cancellationToken),
            _ => true
        };

    private async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        DomainException exception,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        httpContext.Response.StatusCode = (int)exception.StatusCode;

        var problemDetailsContext = new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails { Detail = exception.Message }
        };

        return await problemDetailsService.TryWriteAsync(problemDetailsContext);
    }

    private async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        ValidationException exception,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        httpContext.Response.StatusCode = StatusCodes.Status412PreconditionFailed;

        var problemDetailsContext = new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails
            {
                Detail = exception.Message,
                Extensions = new Dictionary<string, object?>
                {
                    { nameof(exception.Errors).ToLowerInvariant(), exception.Errors }
                }
            }
        };

        return await problemDetailsService.TryWriteAsync(problemDetailsContext);
    }
}
