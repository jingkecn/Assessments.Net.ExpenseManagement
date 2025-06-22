using System.Net;
using System.Net.Http.Json;
using Assessments.ExpenseManagement.Application.Queries;
using Assessments.ExpenseManagement.FunctionalTests.Query.Api.Abstractions;
using Assessments.ExpenseManagement.FunctionalTests.Query.Api.Fixtures;
using Assessments.ExpenseManagement.FunctionalTests.Query.Api.Models;
using Assessments.ExpenseManagement.Infrastructure.Presets;
using Shouldly;

namespace Assessments.ExpenseManagement.FunctionalTests.Query.Api.Endpoints;

public sealed class GetEmployeesTests(TestWebApplicationFactory factory) : TestBase(factory)
{
    [Fact]
    public async Task Should_ReturnOk_When_Successful()
    {
        // Arrange
        var expected = EmployeePresets.PredefinedEmployees.ToList();
        var (pageNumber, pageSize) = new GetEmployeesQuery(1, int.MaxValue);

        // Act
        var response =
            await HttpClient.GetAsync($"api/employees?{nameof(pageNumber)}={pageNumber}&{nameof(pageSize)}={pageSize}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<TestResponse>();
        var actual = content.ShouldNotBeNull().Employees.ShouldNotBeNull().ToList();
        actual.Select(e => e.FirstName).ShouldBe(expected.Select(e => e.FirstName), true);
        actual.Select(e => e.LastName).ShouldBe(expected.Select(e => e.LastName), true);
    }

    [Fact]
    public async Task Should_ReturnPreconditionFailed_When_QueryIsInvalid()
    {
        // Arrange
        var (pageNumber, pageSize) = new GetEmployeesQuery(-1, -1);

        // Act
        var response =
            await HttpClient.GetAsync($"api/employees?{nameof(pageNumber)}={pageNumber}&{nameof(pageSize)}={pageSize}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.PreconditionFailed);
    }

    private sealed record TestResponse
    {
        public IEnumerable<TestEmployee> Employees { get; init; } = null!;
    }
}
