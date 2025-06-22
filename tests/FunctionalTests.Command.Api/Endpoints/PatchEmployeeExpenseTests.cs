using System.Net;
using System.Net.Http.Json;
using Assessments.ExpenseManagement.Application.Models;
using Assessments.ExpenseManagement.Command.Api.Models;
using Assessments.ExpenseManagement.Domain.Models;
using Assessments.ExpenseManagement.FunctionalTests.Command.Api.Abstractions;
using Assessments.ExpenseManagement.FunctionalTests.Command.Api.Fixtures;
using Assessments.ExpenseManagement.Infrastructure.Presets;
using Shouldly;

namespace Assessments.ExpenseManagement.FunctionalTests.Command.Api.Endpoints;

public sealed class PatchEmployeeExpenseTests(TestWebApplicationFactory factory) : TestBase(factory)
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

        var requestForPatch = new PatchEmployeeExpenseRequest(nameof(Expense.Submit));

        // Act
        var response =
            await HttpClient.PatchAsJsonAsync($"api/employees/{employee.Id}/expenses/{expenseId}", requestForPatch);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Should_ReturnNotFound_When_EmployeeNotFound()
    {
        // Arrange
        var employeeId = Guid.NewGuid();

        var requestForPatch = new PatchEmployeeExpenseRequest(nameof(Expense.Submit));

        // Act
        var response =
            await HttpClient.PatchAsJsonAsync($"api/employees/{employeeId}/expenses/{Guid.Empty}", requestForPatch);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Should_ReturnNotFound_When_EmployeeExpenseNotFound()
    {
        // Arrange
        var employee = EmployeePresets.JohnDoe;
        var expenseId = Guid.NewGuid();

        var requestForPatch = new PatchEmployeeExpenseRequest(nameof(Expense.Submit));

        // Act
        var response =
            await HttpClient.PatchAsJsonAsync($"api/employees/{employee.Id}/expenses/{expenseId}", requestForPatch);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Should_ReturnPreconditionFailed_When_RequestIsInvalid()
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

        var requestForPatch = new PatchEmployeeExpenseRequest(
            "Copy" // Invalid action
        );

        // Act
        var response =
            await HttpClient.PatchAsJsonAsync($"api/employees/{employee.Id}/expenses/{expenseId}", requestForPatch);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.PreconditionFailed);
    }
}
