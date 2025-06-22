using System.ComponentModel.DataAnnotations;
using Assessments.ExpenseManagement.Application.Commands;
using Mediator;
using Microsoft.AspNetCore.Mvc;

namespace Assessments.ExpenseManagement.Command.Api.Endpoints;

internal static class ExpenseCommandEndpoints
{
    public static RouteGroupBuilder MapExpenseCommandEndpoints(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("/api/expenses").WithTags("Expense Management");

        api
            .MapDelete("/{id:guid}", DeleteExpenseAsync)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);

        return api;
    }

    private static async Task<IResult> DeleteExpenseAsync(
        [Required] Guid id,
        [FromServices] IMediator mediator = null!,
        CancellationToken cancellationToken = default)
    {
        var command = new DeleteExpenseCommand(id);
        _ = await mediator.Send(command, cancellationToken);
        return TypedResults.NoContent();
    }
}
