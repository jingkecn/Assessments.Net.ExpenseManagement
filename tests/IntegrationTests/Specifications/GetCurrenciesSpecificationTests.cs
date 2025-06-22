using Assessments.ExpenseManagement.Application.Specifications;
using Assessments.ExpenseManagement.Infrastructure.Presets;
using Assessments.ExpenseManagement.IntegrationTests.Abstractions;
using Assessments.ExpenseManagement.IntegrationTests.Fixtures;
using Shouldly;

namespace Assessments.ExpenseManagement.IntegrationTests.Specifications;

[Collection(nameof(TestCollectionFixture))]
public sealed class GetCurrenciesSpecificationTests(TestContainerFactory factory) : TestBaseWithDbMigration(factory)
{
    [Fact]
    public async Task Should_GetCurrencies_When_Applied()
    {
        // Arrange
        var expected = CurrencyPresets.PredefinedCurrencies;
        var specification = new GetCurrenciesSpecification();

        // Act
        var actual = await CurrencyReadRepository.GetAsync(specification);

        // Assert
        actual.ShouldBe(expected, true);
    }
}
