using Assessments.ExpenseManagement.Application.Specifications;
using Assessments.ExpenseManagement.Domain.Models;
using Assessments.ExpenseManagement.Infrastructure.Presets;
using Assessments.ExpenseManagement.IntegrationTests.Abstractions;
using Assessments.ExpenseManagement.IntegrationTests.Fixtures;
using Shouldly;

namespace Assessments.ExpenseManagement.IntegrationTests.Specifications;

[Collection(nameof(TestCollectionFixture))]
public sealed class GetEmployeeExpensesWithAllSpecificationTests(TestContainerFactory factory)
    : TestBaseWithDbMigration(factory)
{
    [Fact]
    public async Task Should_GetEmployeeExpensesWithAll_When_Applied()
    {
        // Arrange
        var employee = EmployeePresets.JohnDoe;
        var expected = new List<Expense>
        {
            Expense.Create(
                employee,
                100M,
                CategoryPresets.Hotel,
                CurrencyPresets.USD,
                DateOnly.FromDateTime(DateTime.Now),
                [Receipt.Create("No comment", $"john.doe/receipt_{Guid.NewGuid()}.jpg")]),
            Expense.Create(
                employee,
                100M,
                CategoryPresets.Restaurant,
                CurrencyPresets.USD,
                DateOnly.FromDateTime(DateTime.Now),
                [Receipt.Create("No comment", $"john.doe/receipt_{Guid.NewGuid()}.jpg")])
        };

        await Context.AddRangeAsync(expected);
        _ = await ExpenseWriteRepository.UnitOfWork.SaveChangesAsync();

        var specification = new GetEmployeeExpensesWithAllSpecification(employee.Id);

        // Act
        var results = await ExpenseReadRepository.GetAsync(specification);

        // Assert
        var actual = results.ToList();
        actual.ShouldBe(expected, true);
        actual.ShouldAllBe(e => e.EmployeeId == employee.Id);
        actual.ShouldAllBe(e => e.Category != null && e.Category.Id == e.CategoryId); // With navigation.
        actual.ShouldAllBe(e => e.Currency != null && e.Currency.Id == e.CurrencyId); // With navigation.
        actual.ShouldAllBe(e => e.Employee != null && e.Employee.Id == e.EmployeeId); // With navigation.

        // Cleanup
        Context.Expenses.RemoveRange(expected);
        _ = await ExpenseWriteRepository.UnitOfWork.SaveChangesAsync();
    }
}
