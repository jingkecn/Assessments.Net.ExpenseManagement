using System.Net;
using System.Net.Http.Json;
using Assessments.ExpenseManagement.FunctionalTests.Query.Api.Abstractions;
using Assessments.ExpenseManagement.FunctionalTests.Query.Api.Fixtures;
using Assessments.ExpenseManagement.FunctionalTests.Query.Api.Models;
using Assessments.ExpenseManagement.Infrastructure.Presets;
using Shouldly;

namespace Assessments.ExpenseManagement.FunctionalTests.Query.Api.Endpoints;

public sealed class GetCategoryByNameTests(TestWebApplicationFactory factory) : TestBase(factory)
{
    [Fact]
    public async Task Should_ReturnOk_When_Successful()
    {
        // Arrange
        var expected = CategoryPresets.Hotel;

        // Act
        var response = await HttpClient.GetAsync($"api/categories/{expected.Name}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<TestResponse>();
        var actual = content.ShouldNotBeNull().Category.ShouldNotBeNull();
        actual.Id.ShouldBe(expected.Id);
    }

    [Fact]
    public async Task Should_ReturnNotFound_When_CategoryNotFound()
    {
        // Arrange
        const string expectedName = "Whatever";

        // Act
        var response = await HttpClient.GetAsync($"api/categories/{expectedName}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    private sealed record TestResponse
    {
        public TestEntity Category { get; init; } = null!;
    }
}
