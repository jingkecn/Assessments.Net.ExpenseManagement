using Assessments.ExpenseManagement.Application.Specifications;
using Assessments.ExpenseManagement.Domain.Models;
using Assessments.ExpenseManagement.Infrastructure.Presets;
using Assessments.ExpenseManagement.IntegrationTests.Abstractions;
using Assessments.ExpenseManagement.IntegrationTests.Fixtures;
using Shouldly;

namespace Assessments.ExpenseManagement.IntegrationTests.Specifications;

[Collection(nameof(TestCollectionFixture))]
public sealed class GetExpensesSpecificationTests(TestContainerFactory factory) : TestBaseWithDbMigration(factory)
{
    [Fact]
    public async Task Should_GetExpenses_When_Applied()
    {
        // Arrange
        var expected = new List<Expense>
        {
            Expense.Create(
                EmployeePresets.JaneDoe,
                100M,
                CategoryPresets.Hotel,
                CurrencyPresets.EUR,
                DateOnly.FromDateTime(DateTime.Now),
                [Receipt.Create("No comment", $"jane.doe/receipt_{Guid.NewGuid()}.jpg")]),
            Expense.Create(
                EmployeePresets.JohnDoe,
                100M,
                CategoryPresets.Restaurant,
                CurrencyPresets.USD,
                DateOnly.FromDateTime(DateTime.Now),
                [Receipt.Create("No comment", $"john.doe/receipt_{Guid.NewGuid()}.jpg")])
        };

        await Context.AddRangeAsync(expected);
        _ = await ExpenseWriteRepository.UnitOfWork.SaveChangesAsync();

        var specification = new GetExpensesSpecification();

        // Act
        var results = await ExpenseReadRepository.GetAsync(specification);

        // Assert
        var actual = results.ToList();
        actual.ShouldBe(expected, true);
        actual.ShouldAllBe(e => e.Category == null); // No navigation.
        actual.ShouldAllBe(e => e.Currency == null); // No navigation.
        actual.ShouldAllBe(e => e.Employee == null); // No navigation.

        // Cleanup
        Context.Expenses.RemoveRange(expected);
        _ = await ExpenseWriteRepository.UnitOfWork.SaveChangesAsync();
    }
}
