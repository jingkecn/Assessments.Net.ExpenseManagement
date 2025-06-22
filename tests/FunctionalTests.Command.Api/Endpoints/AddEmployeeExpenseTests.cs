using System.Net;
using System.Net.Http.Json;
using Assessments.ExpenseManagement.Application.Models;
using Assessments.ExpenseManagement.Command.Api.Models;
using Assessments.ExpenseManagement.FunctionalTests.Command.Api.Abstractions;
using Assessments.ExpenseManagement.FunctionalTests.Command.Api.Fixtures;
using Assessments.ExpenseManagement.Infrastructure.Presets;
using Shouldly;

namespace Assessments.ExpenseManagement.FunctionalTests.Command.Api.Endpoints;

public sealed class AddEmployeeExpenseTests(TestWebApplicationFactory factory) : TestBase(factory)
{
    [Fact]
    public async Task Should_ReturnCreated_When_Successful()
    {
        // Arrange
        var employee = EmployeePresets.JohnDoe;
        var requestForAdd = new AddEmployeeExpenseRequest(
            100M,
            CategoryPresets.Hotel.Name,
            CurrencyPresets.CNY.Code,
            DateOnly.FromDateTime(DateTime.Now),
            [new ReceiptView("No comment", $"receipt_{Guid.NewGuid()}")]);

        // Act
        var response = await HttpClient.PostAsJsonAsync($"api/employees/{employee.Id}/expenses", requestForAdd);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        var result = await response.Content.ReadFromJsonAsync<AddEmployeeExpenseResponse>();
        result.ShouldBeOfType<AddEmployeeExpenseResponse>().Id.ShouldNotBe(Guid.Empty);
    }

    [Fact]
    public async Task Should_ReturnNotFound_When_CategoryNotFound()
    {
        var employee = EmployeePresets.JohnDoe;
        var requestForAdd = new AddEmployeeExpenseRequest(
            100M,
            "Whatever",
            CurrencyPresets.CNY.Code,
            DateOnly.FromDateTime(DateTime.Now),
            [new ReceiptView("No comment", $"receipt_{Guid.NewGuid()}")]);

        // Act
        var response = await HttpClient.PostAsJsonAsync($"api/employees/{employee.Id}/expenses", requestForAdd);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Should_ReturnNotFound_When_CurrencyNotFound()
    {
        var employee = EmployeePresets.JohnDoe;
        var requestForAdd = new AddEmployeeExpenseRequest(
            100M,
            CategoryPresets.Hotel.Name,
            "JPY",
            DateOnly.FromDateTime(DateTime.Now),
            [new ReceiptView("No comment", $"receipt_{Guid.NewGuid()}")]);

        // Act
        var response = await HttpClient.PostAsJsonAsync($"api/employees/{employee.Id}/expenses", requestForAdd);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Should_ReturnPreconditionFailed_When_RequestIsInvalid()
    {
        var employee = EmployeePresets.JohnDoe;
        var requestForAdd = new AddEmployeeExpenseRequest(
            -100M, // Invalid amount value
            CategoryPresets.Hotel.Name,
            CurrencyPresets.CNY.Code,
            DateOnly.FromDateTime(DateTime.Now),
            [new ReceiptView("No comment", $"receipt_{Guid.NewGuid()}")]);

        // Act
        var response = await HttpClient.PostAsJsonAsync($"api/employees/{employee.Id}/expenses", requestForAdd);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.PreconditionFailed);
    }
}
