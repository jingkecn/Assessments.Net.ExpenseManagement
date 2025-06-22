using System.ComponentModel.DataAnnotations;
using Assessments.ExpenseManagement.Application.Queries;
using Assessments.ExpenseManagement.Query.Api.Models;
using Mediator;
using Microsoft.AspNetCore.Mvc;

namespace Assessments.ExpenseManagement.Query.Api.Endpoints;

internal static class CurrencyQueryEndpoints
{
    public static RouteGroupBuilder MapCurrencyQueryEndpoints(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("/api/currencies").WithTags("Currency Management");

        api
            .MapGet("/", GetCurrenciesAsync)
            .Produces<GetCategoriesResponse>();

        api
            .MapGet("/{code}", GetCurrencyByCodeAsync)
            .Produces<GetCurrencyByCodeResponse>()
            .Produces(StatusCodes.Status404NotFound);

        return api;
    }

    private static async Task<IResult> GetCurrencyByCodeAsync(
        [Required] string code,
        [FromServices] IMediator mediator = null!,
        CancellationToken cancellationToken = default)
    {
        var query = new GetCurrencyByCodeQuery(code);
        var response = await mediator.Send(query, cancellationToken);
        return TypedResults.Ok(new GetCurrencyByCodeResponse(response));
    }

    private static async Task<IResult> GetCurrenciesAsync(
        [FromServices] IMediator mediator = null!,
        CancellationToken cancellationToken = default)
    {
        var query = new GetCurrenciesQuery();
        var response = await mediator.Send(query, cancellationToken);
        return TypedResults.Ok(new GetCurrenciesResponse(response));
    }
}
