using Assessments.ExpenseManagement.Application.Specifications;
using Assessments.ExpenseManagement.Infrastructure.Presets;
using Assessments.ExpenseManagement.IntegrationTests.Abstractions;
using Assessments.ExpenseManagement.IntegrationTests.Fixtures;
using Shouldly;

namespace Assessments.ExpenseManagement.IntegrationTests.Specifications;

[Collection(nameof(TestCollectionFixture))]
public sealed class GetCategoryByNameSpecificationTests(TestContainerFactory factory) : TestBaseWithDbMigration(factory)
{
    [Fact]
    public async Task Should_GetCategoryByName_When_Applied()
    {
        // Arrange
        var expected = CategoryPresets.Hotel;
        var specification = new GetCategoryByNameSpecification(expected.Name);

        // Act
        var results = await CategoryReadRepository.GetAsync(specification);

        // Assert
        var actual = results.Single();
        actual.ShouldBe(expected);
    }
}
