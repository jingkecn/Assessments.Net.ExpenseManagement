using Assessments.ExpenseManagement.Application.Specifications;
using Assessments.ExpenseManagement.Infrastructure.Presets;
using Assessments.ExpenseManagement.IntegrationTests.Abstractions;
using Assessments.ExpenseManagement.IntegrationTests.Fixtures;
using Shouldly;

namespace Assessments.ExpenseManagement.IntegrationTests.Specifications;

[Collection(nameof(TestCollectionFixture))]
public sealed class GetCurrencyByCodeSpecificationTests(TestContainerFactory factory) : TestBaseWithDbMigration(factory)
{
    [Fact]
    public async Task Should_GetCurrencyByCode_When_Applied()
    {
        // Arrange
        var expected = CurrencyPresets.CNY;
        var specification = new GetCurrencyByCodeSpecification(expected.Code);

        // Act
        var results = await CurrencyReadRepository.GetAsync(specification);

        // Assert
        var actual = results.Single();
        actual.ShouldBe(expected);
    }
}
