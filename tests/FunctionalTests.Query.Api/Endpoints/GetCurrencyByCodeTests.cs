using System.Net;
using System.Net.Http.Json;
using Assessments.ExpenseManagement.FunctionalTests.Query.Api.Abstractions;
using Assessments.ExpenseManagement.FunctionalTests.Query.Api.Fixtures;
using Assessments.ExpenseManagement.FunctionalTests.Query.Api.Models;
using Assessments.ExpenseManagement.Infrastructure.Presets;
using Shouldly;

namespace Assessments.ExpenseManagement.FunctionalTests.Query.Api.Endpoints;

public sealed class GetCurrencyByCodeTests(TestWebApplicationFactory factory) : TestBase(factory)
{
    [Fact]
    public async Task Should_ReturnOk_When_Successful()
    {
        // Arrange
        var expected = CurrencyPresets.CNY;

        // Act
        var response = await HttpClient.GetAsync($"api/currencies/{expected.Code}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<TestResponse>();
        var actual = content.ShouldNotBeNull().Currency.ShouldNotBeNull();
        actual.Id.ShouldBe(expected.Id);
    }

    [Fact]
    public async Task Should_ReturnNotFound_When_CurrencyNotFound()
    {
        // Arrange
        const string expectedCode = "JPY";

        // Act
        var response = await HttpClient.GetAsync($"api/currencies/{expectedCode}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    private sealed record TestResponse
    {
        public TestEntity Currency { get; init; } = null!;
    }
}
