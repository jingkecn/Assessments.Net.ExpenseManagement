using System.ComponentModel.DataAnnotations;
using Assessments.ExpenseManagement.Application.Queries;
using Assessments.ExpenseManagement.Query.Api.Models;
using Mediator;
using Microsoft.AspNetCore.Mvc;

namespace Assessments.ExpenseManagement.Query.Api.Endpoints;

internal static class EmployeeExpenseQueryEndpoints
{
    public static RouteGroupBuilder MapEmployeeExpenseQueryEndpoints(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("/api/employees").WithTags("Expense Management (Employee)");

        api
            .MapGet("{employeeId:guid}/expenses", GetEmployeeExpensesAsync)
            .Produces<GetEmployeeExpensesResponse>()
            .Produces(StatusCodes.Status404NotFound);

        api
            .MapGet("{employeeId:guid}/expenses/{id:guid}", GetEmployeeExpenseByIdAsync)
            .Produces<GetEmployeeExpenseByIdResponse>()
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status412PreconditionFailed);

        return api;
    }

    private static async Task<IResult> GetEmployeeExpenseByIdAsync(
        [Required] Guid employeeId,
        [Required] Guid id,
        [FromServices] IMediator mediator = null!,
        CancellationToken cancellationToken = default)
    {
        var query = new GetEmployeeExpenseByIdQuery(employeeId, id);
        var response = await mediator.Send(query, cancellationToken);
        return TypedResults.Ok(new GetEmployeeExpenseByIdResponse(response));
    }

    private static async Task<IResult> GetEmployeeExpensesAsync(
        [Required] Guid employeeId,
        [Required] [Range(1, int.MaxValue)] int pageNumber,
        [Required] [Range(1, int.MaxValue)] int pageSize,
        [FromServices] IMediator mediator = null!,
        CancellationToken cancellationToken = default)
    {
        var query = new GetEmployeeExpensesQuery(employeeId, pageNumber, pageSize);
        var (expenses, totalCount) = await mediator.Send(query, cancellationToken);
        return TypedResults.Ok(new GetEmployeeExpensesResponse(expenses, pageNumber, pageSize, totalCount));
    }
}
