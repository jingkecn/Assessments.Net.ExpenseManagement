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

public sealed class GetExpensesTests(TestWebApplicationFactory factory) : TestBase(factory)
{
    [Fact]
    public async Task Should_ReturnOk_When_Successful()
    {
        // Arrange
        var expected = new List<Expense>
        {
            Expense.Create(
                EmployeePresets.JaneDoe,
                123M,
                CategoryPresets.Hotel,
                CurrencyPresets.EUR,
                DateOnly.FromDateTime(DateTime.Now),
                [Receipt.Create("No comment", $"jane.doe/receipt_{Guid.NewGuid()}")]),
            Expense.Create(
                EmployeePresets.JohnDoe,
                456M,
                CategoryPresets.Restaurant,
                CurrencyPresets.USD,
                DateOnly.FromDateTime(DateTime.Now),
                [Receipt.Create("No comment", $"john.doe/receipt_{Guid.NewGuid()}")])
        };

        await Context.Expenses.AddRangeAsync(expected);
        _ = await Context.SaveChangesAsync();

        var (pageNumber, pageSize) = new GetExpensesQuery(1, int.MaxValue);

        // Act
        var response =
            await HttpClient.GetAsync($"api/expenses?{nameof(pageNumber)}={pageNumber}&{nameof(pageSize)}={pageSize}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<TestResponse>();
        var actual = content.ShouldNotBeNull().Expenses.ShouldNotBeNull().ToList();
        var views = expected.ToView().ToList();
        actual.Select(e => e.Amount).ShouldBe(views.Select(e => e.Amount), true);
        actual.Select(e => e.Employee).ShouldBe(views.Select(e => e.Employee), true);
        actual.Select(e => e.ExecutionDate).ShouldBe(views.Select(e => e.ExecutionDate), true);
        actual.Select(e => e.Status).ShouldBe(views.Select(e => e.Status), true);

        // Cleanup
        Context.Expenses.RemoveRange(expected);
        _ = await Context.SaveChangesAsync();
    }

    [Fact]
    public async Task Should_ReturnPreconditionFailed_When_QueryIsInvalid()
    {
        // Arrange
        var (pageNumber, pageSize) = new GetExpensesQuery(-1, -1);

        // Act
        var response =
            await HttpClient.GetAsync($"api/expenses?{nameof(pageNumber)}={pageNumber}&{nameof(pageSize)}={pageSize}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.PreconditionFailed);
    }

    private sealed record TestResponse
    {
        public IEnumerable<TestExpense> Expenses { get; init; } = null!;
    }
}
