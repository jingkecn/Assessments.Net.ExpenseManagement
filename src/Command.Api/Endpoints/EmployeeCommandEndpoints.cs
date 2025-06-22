using System.ComponentModel.DataAnnotations;
using Assessments.ExpenseManagement.Application.Commands;
using Assessments.ExpenseManagement.Command.Api.Models;
using Mediator;
using Microsoft.AspNetCore.Mvc;

namespace Assessments.ExpenseManagement.Command.Api.Endpoints;

internal static class EmployeeCommandEndpoints
{
    public static RouteGroupBuilder MapEmployeeCommandEndpoints(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("/api/employees").WithTags("Employee Management");

        api
            .MapPost("/", AddEmployeeAsync)
            .Produces<AddEmployeeResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status412PreconditionFailed);

        api
            .MapDelete("/{id:guid}", DeleteEmployeeAsync)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);

        api
            .MapPut("/{id:guid}", UpdateEmployeeAsync)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status412PreconditionFailed);

        return api;
    }

    private static async Task<IResult> AddEmployeeAsync(
        [FromBody] AddEmployeeRequest request,
        [FromServices] IMediator mediator = null!,
        CancellationToken cancellationToken = default)
    {
        var command = new AddEmployeeCommand(request.FirstName, request.LastName, request.CurrencyCode);
        var response = await mediator.Send(command, cancellationToken);
        return TypedResults.Created($"api/employees/{response}", new AddEmployeeResponse(response));
    }

    private static async Task<IResult> DeleteEmployeeAsync(
        [Required] Guid id,
        [FromServices] IMediator mediator = null!,
        CancellationToken cancellationToken = default)
    {
        var command = new DeleteEmployeeCommand(id);
        _ = await mediator.Send(command, cancellationToken);
        return TypedResults.NoContent();
    }

    private static async Task<IResult> UpdateEmployeeAsync(
        [Required] Guid id,
        [FromBody] UpdateEmployeeRequest request,
        [FromServices] IMediator mediator = null!,
        CancellationToken cancellationToken = default)
    {
        var command = new UpdateEmployeeCommand(id, request.CurrencyCode);
        _ = await mediator.Send(command, cancellationToken);
        return TypedResults.NoContent();
    }
}
