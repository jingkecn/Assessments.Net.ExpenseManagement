using Assessments.ExpenseManagement.Application.Specifications;
using Assessments.ExpenseManagement.Infrastructure.Presets;
using Assessments.ExpenseManagement.IntegrationTests.Abstractions;
using Assessments.ExpenseManagement.IntegrationTests.Fixtures;
using Shouldly;

namespace Assessments.ExpenseManagement.IntegrationTests.Specifications;

[Collection(nameof(TestCollectionFixture))]
public sealed class GetEmployeeByIdWithCurrencySpecificationTests(TestContainerFactory factory)
    : TestBaseWithDbMigration(factory)
{
    [Fact]
    public async Task Should_GetEmployeeByIdWithCurrency_When_Applied()
    {
        // Arrange
        var expected = EmployeePresets.JohnDoe;
        var specification = new GetEmployeeByIdWithCurrencySpecification(expected.Id);

        // Act
        var results = await EmployeeReadRepository.GetAsync(specification);

        // Assert
        var actual = results.Single();
        actual.ShouldBe(expected);
        actual.Currency.ShouldNotBeNull().Id.ShouldBe(expected.CurrencyId);
    }
}
