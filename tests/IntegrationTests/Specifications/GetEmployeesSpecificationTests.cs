using Assessments.ExpenseManagement.Application.Specifications;
using Assessments.ExpenseManagement.Infrastructure.Presets;
using Assessments.ExpenseManagement.IntegrationTests.Abstractions;
using Assessments.ExpenseManagement.IntegrationTests.Fixtures;
using Shouldly;

namespace Assessments.ExpenseManagement.IntegrationTests.Specifications;

[Collection(nameof(TestCollectionFixture))]
public sealed class GetEmployeesSpecificationTests(TestContainerFactory factory) : TestBaseWithDbMigration(factory)
{
    [Fact]
    public async Task Should_GetEmployees_When_Applied()
    {
        // Arrange
        var expected = EmployeePresets.PredefinedEmployees;
        var specification = new GetEmployeesSpecification();

        // Act
        var results = await EmployeeReadRepository.GetAsync(specification);

        // Assert
        var actual = results.ToList();
        actual.ShouldBe(expected, true);
        actual.ShouldAllBe(e => e.Currency == null); // No navigation.
    }
}
