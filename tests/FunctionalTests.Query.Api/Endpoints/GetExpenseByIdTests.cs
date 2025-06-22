using System.Net;
using System.Net.Http.Json;
using Assessments.ExpenseManagement.Application.Extensions;
using Assessments.ExpenseManagement.Domain.Models;
using Assessments.ExpenseManagement.FunctionalTests.Query.Api.Abstractions;
using Assessments.ExpenseManagement.FunctionalTests.Query.Api.Fixtures;
using Assessments.ExpenseManagement.FunctionalTests.Query.Api.Models;
using Assessments.ExpenseManagement.Infrastructure.Presets;
using Shouldly;

namespace Assessments.ExpenseManagement.FunctionalTests.Query.Api.Endpoints;

public sealed class GetExpenseByIdTests(TestWebApplicationFactory factory) : TestBase(factory)
{
    [Fact]
    public async Task Should_ReturnOk_When_Successful()
    {
        // Arrange
        var expected = Expense.Create(
            EmployeePresets.JaneDoe,
            100M,
            CategoryPresets.Hotel,
            CurrencyPresets.EUR,
            DateOnly.FromDateTime(DateTime.Now),
            [Receipt.Create("No comment", $"jane.doe/receipt_{Guid.NewGuid()}")]);

        _ = await Context.Expenses.AddAsync(expected);
        _ = await Context.SaveChangesAsync();

        // Act
        var response = await HttpClient.GetAsync($"api/expenses/{expected.Id}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<TestResponse>();
        var actual = content.ShouldNotBeNull().Expense.ShouldNotBeNull();
        var view = expected.ToView();
        actual.Amount.ShouldBe(view.Amount);
        actual.Employee.ShouldBe(view.Employee);
        actual.ExecutionDate.ShouldBe(view.ExecutionDate);
        actual.Status.ShouldBe(view.Status);
        actual.SubmissionDate.ShouldBe(view.SubmissionDate);

        // Cleanup
        _ = Context.Expenses.Remove(expected);
        _ = await Context.SaveChangesAsync();
    }

    [Fact]
    public async Task Should_ReturnNotFound_When_ExpenseNotFound()
    {
        // Arrange
        var expectedId = Guid.NewGuid();

        // Act
        var response = await HttpClient.GetAsync($"api/expenses/{expectedId}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    private sealed record TestResponse
    {
        public TestExpense Expense { get; init; } = null!;
    }
}
