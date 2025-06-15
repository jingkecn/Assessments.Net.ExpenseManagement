using System.ComponentModel.DataAnnotations;
using Assessments.ExpenseManagement.Application.Models;
using Assessments.ExpenseManagement.Application.UseCases.Get;
using Assessments.ExpenseManagement.Query.Api.Models;
using Mediator;
using Microsoft.AspNetCore.Mvc;

namespace Assessments.ExpenseManagement.Query.Api.Endpoints;

public static class ExpenseQueryEndpoints
{
    public static RouteGroupBuilder MapExpenseQueryEndpoints(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("/api/expenses").WithTags("Expense Management");

        api
            .MapGet("", GetAsync)
            .Produces<GetExpenseResponse>()
            .Produces<GetExpensesResponse>()
            .Produces(StatusCodes.Status404NotFound);

        return api;
    }

    private static async Task<IResult> GetAsync(
        [Required] [AsParameters] GetExpenseRequest request,
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken = default) =>
        request switch
        {
            var (id, _, _, _) when id != Guid.Empty =>
                await GetExpenseByIdAsync(id, mediator, cancellationToken),
            var (id, userId, sortBy, sortDirection) when id == Guid.Empty && userId != Guid.Empty =>
                await GetExpensesByUserIdAsync(userId, sortBy, sortDirection, mediator, cancellationToken),
            var (id, userId, sortBy, sortDirection) when id == Guid.Empty && userId == Guid.Empty =>
                await GetExpensesAsync(sortBy, sortDirection, mediator, cancellationToken),
            _ => throw new ArgumentOutOfRangeException(nameof(request))
        };

    private static async Task<IResult> GetExpenseByIdAsync(
        [Required] Guid id,
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken = default)
    {
        var query = new GetExpenseByIdQuery(id);
        var response = await mediator.Send(query, cancellationToken);
        return TypedResults.Ok(new GetExpenseResponse(response));
    }

    private static async Task<IResult> GetExpensesAsync(
        [Required] SortBy sortBy,
        [Required] SortDirection sortDirection,
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken = default)
    {
        var query = new GetExpensesQuery(sortBy, sortDirection);
        var response = await mediator.Send(query, cancellationToken);
        return TypedResults.Ok(new GetExpensesResponse(response));
    }

    private static async Task<IResult> GetExpensesByUserIdAsync(
        [Required] Guid userId,
        [Required] SortBy sortBy,
        [Required] SortDirection sortDirection,
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken = default)
    {
        var query = new GetExpensesByUserIdQuery(userId, sortBy, sortDirection);
        var response = await mediator.Send(query, cancellationToken);
        return TypedResults.Ok(new GetExpensesResponse(response));
    }
}
