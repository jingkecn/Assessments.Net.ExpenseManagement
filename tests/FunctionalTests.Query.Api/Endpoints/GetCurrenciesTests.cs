using System.Net;
using System.Net.Http.Json;
using Assessments.ExpenseManagement.FunctionalTests.Query.Api.Abstractions;
using Assessments.ExpenseManagement.FunctionalTests.Query.Api.Fixtures;
using Assessments.ExpenseManagement.FunctionalTests.Query.Api.Models;
using Assessments.ExpenseManagement.Infrastructure.Presets;
using Shouldly;

namespace Assessments.ExpenseManagement.FunctionalTests.Query.Api.Endpoints;

public sealed class GetCurrenciesTests(TestWebApplicationFactory factory) : TestBase(factory)
{
    [Fact]
    public async Task Should_ReturnOk_When_Successful()
    {
        // Arrange
        var expected = CurrencyPresets.PredefinedCurrencies;

        // Act
        var response = await HttpClient.GetAsync("api/currencies");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<TestResponse>();
        var actual = content.ShouldNotBeNull().Currencies.ShouldNotBeNull();
        actual.Select(c => c.Id).ShouldBe(expected.Select(c => c.Id), true);
    }

    private sealed record TestResponse
    {
        public IEnumerable<TestEntity> Currencies { get; init; } = null!;
    }
}
