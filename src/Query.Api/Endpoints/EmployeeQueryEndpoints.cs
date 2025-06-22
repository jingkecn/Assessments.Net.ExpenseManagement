using System.ComponentModel.DataAnnotations;
using Assessments.ExpenseManagement.Application.Queries;
using Assessments.ExpenseManagement.Query.Api.Models;
using Mediator;
using Microsoft.AspNetCore.Mvc;

namespace Assessments.ExpenseManagement.Query.Api.Endpoints;

internal static class EmployeeQueryEndpoints
{
    public static RouteGroupBuilder MapEmployeeQueryEndpoints(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("/api/employees").WithTags("Employee Management");

        api
            .MapGet("/", GetEmployeesAsync)
            .Produces<GetEmployeesResponse>()
            .Produces(StatusCodes.Status412PreconditionFailed);

        api
            .MapGet("/{id:guid}", GetEmployeeByIdAsync)
            .Produces<GetEmployeeByIdResponse>()
            .Produces(StatusCodes.Status404NotFound);


        return api;
    }

    private static async Task<IResult> GetEmployeeByIdAsync(
        [Required] Guid id,
        [FromServices] IMediator mediator = null!,
        CancellationToken cancellationToken = default)
    {
        var query = new GetEmployeeByIdQuery(id);
        var response = await mediator.Send(query, cancellationToken);
        return TypedResults.Ok(new GetEmployeeByIdResponse(response));
    }

    private static async Task<IResult> GetEmployeesAsync(
        [Required] [Range(1, int.MaxValue)] int pageNumber,
        [Required] [Range(1, int.MaxValue)] int pageSize,
        [FromServices] IMediator mediator = null!,
        CancellationToken cancellationToken = default)
    {
        var query = new GetEmployeesQuery(pageNumber, pageSize);
        var (employees, totalCount) = await mediator.Send(query, cancellationToken);
        return TypedResults.Ok(new GetEmployeesResponse(employees, pageNumber, pageSize, totalCount));
    }
}
