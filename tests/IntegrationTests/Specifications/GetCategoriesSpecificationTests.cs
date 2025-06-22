using Assessments.ExpenseManagement.Application.Specifications;
using Assessments.ExpenseManagement.Infrastructure.Presets;
using Assessments.ExpenseManagement.IntegrationTests.Abstractions;
using Assessments.ExpenseManagement.IntegrationTests.Fixtures;
using Shouldly;

namespace Assessments.ExpenseManagement.IntegrationTests.Specifications;

[Collection(nameof(TestCollectionFixture))]
public sealed class GetCategoriesSpecificationTests(TestContainerFactory factory) : TestBaseWithDbMigration(factory)
{
    [Fact]
    public async Task Should_GetCategories_When_Applied()
    {
        // Arrange
        var expected = CategoryPresets.PredefinedCategories;
        var specification = new GetCategoriesSpecification();

        // Act
        var actual = await CategoryReadRepository.GetAsync(specification);

        // Assert
        actual.ShouldBe(expected, true);
    }
}
