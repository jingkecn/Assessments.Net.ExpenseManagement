using System.ComponentModel.DataAnnotations;
using Assessments.ExpenseManagement.Application.Queries;
using Assessments.ExpenseManagement.Query.Api.Models;
using Mediator;
using Microsoft.AspNetCore.Mvc;

namespace Assessments.ExpenseManagement.Query.Api.Endpoints;

internal static class CategoryQueryEndpoints
{
    public static RouteGroupBuilder MapCategoryQueryEndpoints(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("/api/categories").WithTags("Category Management");

        api
            .MapGet("/", GetCategoriesAsync)
            .Produces<GetCategoriesResponse>();

        api
            .MapGet("/{name}", GetCategoryByNameAsync)
            .Produces<GetCategoryByNameResponse>()
            .Produces(StatusCodes.Status404NotFound);

        return api;
    }

    private static async Task<IResult> GetCategoryByNameAsync(
        [Required] string name,
        [FromServices] IMediator mediator = null!,
        CancellationToken cancellationToken = default)
    {
        var query = new GetCategoryByNameQuery(name);
        var response = await mediator.Send(query, cancellationToken);
        return TypedResults.Ok(new GetCategoryByNameResponse(response));
    }

    private static async Task<IResult> GetCategoriesAsync(
        [FromServices] IMediator mediator = null!,
        CancellationToken cancellationToken = default)
    {
        var query = new GetCategoriesQuery();
        var response = await mediator.Send(query, cancellationToken);
        return TypedResults.Ok(new GetCategoriesResponse(response));
    }
}
