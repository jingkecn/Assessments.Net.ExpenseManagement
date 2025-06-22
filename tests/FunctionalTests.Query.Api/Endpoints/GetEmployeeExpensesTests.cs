using System.Net;
using System.Net.Http.Json;
using Assessments.ExpenseManagement.Application.Extensions;
using Assessments.ExpenseManagement.Application.Queries;
using Assessments.ExpenseManagement.Domain.Models;
using Assessments.ExpenseManagement.FunctionalTests.Query.Api.Abstractions;
using Assessments.ExpenseManagement.FunctionalTests.Query.Api.Fixtures;
using Assessments.ExpenseManagement.FunctionalTests.Query.Api.Models;
using Assessments.ExpenseManagement.Infrastructure.Presets;
using Shouldly;

namespace Assessments.ExpenseManagement.FunctionalTests.Query.Api.Endpoints;

public sealed class GetEmployeeExpensesTests(TestWebApplicationFactory factory) : TestBase(factory)
{
    [Fact]
    public async Task Should_ReturnOk_When_Successful()
    {
        // Arrange
        var employee = EmployeePresets.JohnDoe;
        var expected = new List<Expense>
        {
            Expense.Create(
                employee,
                123M,
                CategoryPresets.Hotel,
                CurrencyPresets.USD,
                DateOnly.FromDateTime(DateTime.Now),
                [Receipt.Create("No comment", $"john.doe/receipt_{Guid.NewGuid()}")]),
            Expense.Create(
                employee,
                456M,
                CategoryPresets.Restaurant,
                CurrencyPresets.USD,
                DateOnly.FromDateTime(DateTime.Now),
                [Receipt.Create("No comment", $"john.doe/receipt_{Guid.NewGuid()}")])
        };

        await Context.Expenses.AddRangeAsync(expected);
        _ = await Context.SaveChangesAsync();

        var (_, pageNumber, pageSize) = new GetEmployeeExpensesQuery(employee.Id, 1, int.MaxValue);

        // Act
        var response = await HttpClient.GetAsync(
            $"api/employees/{employee.Id}/expenses?{nameof(pageNumber)}={pageNumber}&{nameof(pageSize)}={pageSize}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<TestResponse>();
        var actual = content.ShouldNotBeNull().Expenses.ShouldNotBeNull().ToList();
        var views = expected.ToView().ToList();
        actual.Select(e => e.Amount).ShouldBe(views.Select(e => e.Amount), true);
        actual.Select(e => e.Employee).ShouldBe(views.Select(e => e.Employee), true);
        actual.Select(e => e.ExecutionDate).ShouldBe(expected.Select(e => e.ExecutionDate), true);
        actual.Select(e => e.Status).ShouldBe(expected.Select(e => e.Status.ToString()), true);
        actual.Select(e => e.SubmissionDate).ShouldBe(expected.Select(e => e.SubmissionDate), true);

        // Cleanup
        Context.Expenses.RemoveRange(expected);
        _ = await Context.SaveChangesAsync();
    }

    [Fact]
    public async Task Should_ReturnNotFound_When_EmployeeNotFound()
    {
        // Arrange
        var employeeId = Guid.NewGuid();
        var (_, pageNumber, pageSize) = new GetEmployeeExpensesQuery(employeeId, 1, int.MaxValue);

        // Act
        var response = await HttpClient.GetAsync(
            $"api/employees/{employeeId}/expenses?{nameof(pageNumber)}={pageNumber}&{nameof(pageSize)}={pageSize}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Should_ReturnPreconditionFailed_When_QueryIsInvalid()
    {
        // Arrange
        var employee = EmployeePresets.JohnDoe;
        var (_, pageNumber, pageSize) = new GetEmployeeExpensesQuery(employee.Id, -1, -1);

        // Act
        var response = await HttpClient.GetAsync(
            $"api/employees/{employee.Id}/expenses?{nameof(pageNumber)}={pageNumber}&{nameof(pageSize)}={pageSize}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.PreconditionFailed);
    }

    private sealed record TestResponse
    {
        public IEnumerable<TestExpense> Expenses { get; init; } = null!;
    }
}
