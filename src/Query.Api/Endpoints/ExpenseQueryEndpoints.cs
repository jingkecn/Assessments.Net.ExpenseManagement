using System.ComponentModel.DataAnnotations;
using Assessments.ExpenseManagement.Application.Queries;
using Assessments.ExpenseManagement.Query.Api.Models;
using Mediator;
using Microsoft.AspNetCore.Mvc;

namespace Assessments.ExpenseManagement.Query.Api.Endpoints;

internal static class ExpenseQueryEndpoints
{
    public static RouteGroupBuilder MapExpenseQueryEndpoints(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("/api/expenses").WithTags("Expense Management");

        api
            .MapGet("/", GetExpensesAsync)
            .Produces<GetExpensesResponse>()
            .Produces(StatusCodes.Status412PreconditionFailed);

        api
            .MapGet("/{id:guid}", GetExpenseByIdAsync)
            .Produces<GetExpenseByIdResponse>()
            .Produces(StatusCodes.Status404NotFound);

        return api;
    }

    private static async Task<IResult> GetExpenseByIdAsync(
        [Required] Guid id,
        [FromServices] IMediator mediator = null!,
        CancellationToken cancellationToken = default)
    {
        var query = new GetExpenseByIdQuery(id);
        var response = await mediator.Send(query, cancellationToken);
        return TypedResults.Ok(new GetExpenseByIdResponse(response));
    }

    private static async Task<IResult> GetExpensesAsync(
        [Required] [Range(1, int.MaxValue)] int pageNumber,
        [Required] [Range(1, int.MaxValue)] int pageSize,
        [FromServices] IMediator mediator = null!,
        CancellationToken cancellationToken = default)
    {
        var query = new GetExpensesQuery(pageNumber, pageSize);
        var (expenses, totalCount) = await mediator.Send(query, cancellationToken);
        return TypedResults.Ok(new GetExpensesResponse(expenses, pageNumber, pageSize, totalCount));
    }
}
