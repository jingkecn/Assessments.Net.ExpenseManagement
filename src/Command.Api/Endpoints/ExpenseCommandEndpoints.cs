using Assessments.ExpenseManagement.Application.UseCases.Create;
using Assessments.ExpenseManagement.Command.Api.Models;
using Mediator;
using Microsoft.AspNetCore.Mvc;

namespace Assessments.ExpenseManagement.Command.Api.Endpoints;

public static class ExpenseCommandEndpoints
{
    public static RouteGroupBuilder MapExpenseCommandEndpoints(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("/api/expenses").WithTags("Expense Management");

        api
            .MapPost("/", CreateExpenseAsync)
            .Produces<CreateExpenseResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status412PreconditionFailed);

        return api;
    }

    private static async Task<IResult> CreateExpenseAsync(
        [FromBody] CreateExpenseRequest request,
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken = default)
    {
        var (amount, categoryName, currencyCode, description, executionDate, userId) = request;
        var command = new CreateExpenseCommand(amount, categoryName, currencyCode, description, executionDate, userId);
        var response = await mediator.Send(command, cancellationToken);
        return TypedResults.Created($"api/expenses/{response}", new CreateExpenseResponse(response));
    }
}
