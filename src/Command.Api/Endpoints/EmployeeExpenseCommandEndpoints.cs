using System.ComponentModel.DataAnnotations;
using Assessments.ExpenseManagement.Application.Commands;
using Assessments.ExpenseManagement.Application.Models;
using Assessments.ExpenseManagement.Command.Api.Models;
using Mediator;
using Microsoft.AspNetCore.Mvc;

namespace Assessments.ExpenseManagement.Command.Api.Endpoints;

internal static class EmployeeExpenseCommandEndpoints
{
    public static RouteGroupBuilder MapEmployeeExpenseCommandEndpoints(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("/api/employees").WithTags("Expense Management (Employee)");

        api
            .MapPost("{employeeId:guid}/expenses/", AddEmployeeExpenseAsync)
            .Produces<AddEmployeeExpenseResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status412PreconditionFailed);

        api
            .MapPatch("{employeeId:guid}/expenses/{expenseId:guid}", PatchEmployeeExpenseAsync)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status412PreconditionFailed);

        return api;
    }

    private static async Task<IResult> AddEmployeeExpenseAsync(
        [Required] Guid employeeId,
        [FromBody] AddEmployeeExpenseRequest request,
        [FromServices] IMediator mediator = null!,
        CancellationToken cancellationToken = default)
    {
        var command = new AddEmployeeExpenseCommand(
            request.Amount,
            request.CategoryName,
            request.CurrencyCode,
            employeeId,
            request.ExecutionDate,
            request.Receipts.Select(r => new ReceiptView(r.Comment, r.FileUrl)));
        var response = await mediator.Send(command, cancellationToken);
        return TypedResults.Created(
            $"api/employees/{employeeId}/expenses/{response}",
            new AddEmployeeExpenseResponse(response));
    }

    private static async Task<IResult> PatchEmployeeExpenseAsync(
        [Required] Guid employeeId,
        [Required] Guid expenseId,
        [FromBody] PatchEmployeeExpenseRequest request,
        [FromServices] IMediator mediator = null!,
        CancellationToken cancellationToken = default)
    {
        var command = new PatchEmployeeExpenseCommand(request.Action, employeeId, expenseId);
        _ = await mediator.Send(command, cancellationToken);
        return TypedResults.NoContent();
    }
}
