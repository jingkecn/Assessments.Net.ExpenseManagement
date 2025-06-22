using System.Net;
using System.Net.Http.Json;
using Assessments.ExpenseManagement.FunctionalTests.Query.Api.Abstractions;
using Assessments.ExpenseManagement.FunctionalTests.Query.Api.Fixtures;
using Assessments.ExpenseManagement.FunctionalTests.Query.Api.Models;
using Assessments.ExpenseManagement.Infrastructure.Presets;
using Shouldly;

namespace Assessments.ExpenseManagement.FunctionalTests.Query.Api.Endpoints;

public sealed class GetCategoriesTests(TestWebApplicationFactory factory) : TestBase(factory)
{
    [Fact]
    public async Task Should_ReturnOk_When_Successful()
    {
        // Arrange
        var expected = CategoryPresets.PredefinedCategories;

        // Act
        var response = await HttpClient.GetAsync("api/categories");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<TestResponse>();
        var actual = content.ShouldNotBeNull().Categories.ShouldNotBeNull();
        actual.Select(c => c.Id).ShouldBe(expected.Select(c => c.Id), true);
    }

    private sealed record TestResponse
    {
        public IEnumerable<TestEntity> Categories { get; init; } = null!;
    }
}
