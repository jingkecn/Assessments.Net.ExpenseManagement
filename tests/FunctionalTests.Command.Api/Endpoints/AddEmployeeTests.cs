using System.Net;
using System.Net.Http.Json;
using Assessments.ExpenseManagement.Command.Api.Models;
using Assessments.ExpenseManagement.Domain.Models;
using Assessments.ExpenseManagement.FunctionalTests.Command.Api.Abstractions;
using Assessments.ExpenseManagement.FunctionalTests.Command.Api.Fixtures;
using Assessments.ExpenseManagement.Infrastructure.Presets;
using Shouldly;

namespace Assessments.ExpenseManagement.FunctionalTests.Command.Api.Endpoints;

public sealed class AddEmployeeTests(TestWebApplicationFactory factory) : TestBase(factory)
{
    [Fact]
    public async Task Should_ReturnCreated_When_Successful()
    {
        // Arrange
        var requestForAdd = new AddEmployeeRequest(
            $"{nameof(Employee.FirstName)}_{Guid.NewGuid()}",
            $"{nameof(Employee.LastName)}_{Guid.NewGuid()}",
            CurrencyPresets.CNY.Code);

        // Act
        var response = await HttpClient.PostAsJsonAsync("api/employees", requestForAdd);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        var result = await response.Content.ReadFromJsonAsync<AddEmployeeResponse>();
        result.ShouldBeOfType<AddEmployeeResponse>().Id.ShouldNotBe(Guid.Empty);
    }

    [Fact]
    public async Task Should_ReturnNotFound_When_CurrencyNotFound()
    {
        // Arrange
        var requestForAdd = new AddEmployeeRequest(
            $"{nameof(Employee.FirstName)}_{Guid.NewGuid()}",
            $"{nameof(Employee.LastName)}_{Guid.NewGuid()}",
            "JPY");

        // Act
        var response = await HttpClient.PostAsJsonAsync("api/employees", requestForAdd);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Should_ReturnPreconditionFailed_When_RequestIsInvalid()
    {
        // Arrange
        var invalidName = string.Join(string.Empty, Enumerable.Repeat('a', 51));
        var requestForAdd = new AddEmployeeRequest(
            invalidName,
            $"{nameof(Employee.LastName)}_{Guid.NewGuid()}",
            "JPY");

        // Act
        var response = await HttpClient.PostAsJsonAsync("api/employees", requestForAdd);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.PreconditionFailed);
    }
}
