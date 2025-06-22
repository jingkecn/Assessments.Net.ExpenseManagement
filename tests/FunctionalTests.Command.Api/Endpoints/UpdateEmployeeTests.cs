using System.Net;
using System.Net.Http.Json;
using Assessments.ExpenseManagement.Command.Api.Models;
using Assessments.ExpenseManagement.Domain.Models;
using Assessments.ExpenseManagement.FunctionalTests.Command.Api.Abstractions;
using Assessments.ExpenseManagement.FunctionalTests.Command.Api.Fixtures;
using Assessments.ExpenseManagement.Infrastructure.Presets;
using Shouldly;

namespace Assessments.ExpenseManagement.FunctionalTests.Command.Api.Endpoints;

public sealed class UpdateEmployeeTests(TestWebApplicationFactory factory) : TestBase(factory)
{
    [Fact]
    public async Task Should_ReturnNoContent_When_Successful()
    {
        // Arrange
        var requestForAdd = new AddEmployeeRequest(
            $"{nameof(Employee.FirstName)}_{Guid.NewGuid()}",
            $"{nameof(Employee.LastName)}_{Guid.NewGuid()}",
            CurrencyPresets.USD.Code);
        var messageForAdd = await HttpClient.PostAsJsonAsync("api/employees", requestForAdd);
        var responseForAdd = await messageForAdd.Content.ReadFromJsonAsync<AddEmployeeResponse>();
        var employeeId = responseForAdd!.Id;

        var requestForUpdate = new UpdateEmployeeRequest(CurrencyPresets.CNY.Code);

        // Act
        var response = await HttpClient.PutAsJsonAsync($"api/employees/{employeeId}", requestForUpdate);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Should_ReturnNotFound_When_CurrencyNotFound()
    {
        // Arrange
        var requestForAdd = new AddEmployeeRequest(
            $"{nameof(Employee.FirstName)}_{Guid.NewGuid()}",
            $"{nameof(Employee.LastName)}_{Guid.NewGuid()}",
            CurrencyPresets.USD.Code);
        var messageForAdd = await HttpClient.PostAsJsonAsync("api/employees", requestForAdd);
        var responseForAdd = await messageForAdd.Content.ReadFromJsonAsync<AddEmployeeResponse>();
        var employeeId = responseForAdd!.Id;

        var requestForUpdate = new UpdateEmployeeRequest("JPY");

        // Act
        var response = await HttpClient.PutAsJsonAsync($"api/employees/{employeeId}", requestForUpdate);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Should_ReturnNotFound_When_EmployeeNotFound()
    {
        // Arrange
        var employeeId = Guid.NewGuid();
        var requestForUpdate = new UpdateEmployeeRequest("JPY");

        // Act
        var response = await HttpClient.PutAsJsonAsync($"api/employees/{employeeId}", requestForUpdate);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
}
