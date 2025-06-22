using System.Net;
using System.Net.Http.Json;
using Assessments.ExpenseManagement.Application.Models;
using Assessments.ExpenseManagement.Command.Api.Models;
using Assessments.ExpenseManagement.FunctionalTests.Command.Api.Abstractions;
using Assessments.ExpenseManagement.FunctionalTests.Command.Api.Fixtures;
using Assessments.ExpenseManagement.Infrastructure.Presets;
using Shouldly;

namespace Assessments.ExpenseManagement.FunctionalTests.Command.Api.Endpoints;

public sealed class DeleteExpenseTests(TestWebApplicationFactory factory) : TestBase(factory)
{
    [Fact]
    public async Task Should_ReturnNoContent_When_Successful()
    {
        // Arrange
        var employee = EmployeePresets.JohnDoe;
        var requestForAdd = new AddEmployeeExpenseRequest(
            100M,
            CategoryPresets.Hotel.Name,
            CurrencyPresets.CNY.Code,
            DateOnly.FromDateTime(DateTime.Now),
            [new ReceiptView("No comment", $"receipt_{Guid.NewGuid()}")]);
        var responseForAdd = await HttpClient.PostAsJsonAsync($"api/employees/{employee.Id}/expenses", requestForAdd);
        var resultForAdd = await responseForAdd.Content.ReadFromJsonAsync<AddEmployeeExpenseResponse>();
        var expenseId = resultForAdd!.Id;

        // Act
        var response = await HttpClient.DeleteAsync($"api/expenses/{expenseId}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Should_ReturnNotFound_When_ExpenseNotFound()
    {
        // Arrange
        var expenseId = Guid.NewGuid();

        // Act
        var response = await HttpClient.DeleteAsync($"api/expenses/{expenseId}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
}
