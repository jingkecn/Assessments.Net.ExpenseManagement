using System.Net;
using System.Net.Http.Json;
using Assessments.ExpenseManagement.Application.Extensions;
using Assessments.ExpenseManagement.FunctionalTests.Query.Api.Abstractions;
using Assessments.ExpenseManagement.FunctionalTests.Query.Api.Fixtures;
using Assessments.ExpenseManagement.FunctionalTests.Query.Api.Models;
using Assessments.ExpenseManagement.Infrastructure.Presets;
using Shouldly;

namespace Assessments.ExpenseManagement.FunctionalTests.Query.Api.Endpoints;

public sealed class GetEmployeeByIdTests(TestWebApplicationFactory factory) : TestBase(factory)
{
    [Fact]
    public async Task Should_ReturnOk_When_Successful()
    {
        // Arrange
        var expected = EmployeePresets.JohnDoe;

        // Act
        var response = await HttpClient.GetAsync($"api/employees/{expected.Id}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<TestResponse>();
        var actual = content.ShouldNotBeNull().Employee.ShouldNotBeNull();
        var view = expected.ToView();
        actual.FirstName.ShouldBe(view.FirstName);
        actual.LastName.ShouldBe(view.LastName);
    }

    [Fact]
    public async Task Should_ReturnNotFound_When_EmployeeNotFound()
    {
        // Arrange
        var expectedId = Guid.NewGuid();

        // Act
        var response = await HttpClient.GetAsync($"api/employees/{expectedId}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    private sealed record TestResponse
    {
        public TestEmployee Employee { get; init; } = null!;
    }
}
