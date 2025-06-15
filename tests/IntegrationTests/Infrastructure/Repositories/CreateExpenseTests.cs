using Assessments.ExpenseManagement.Domain.Models;
using Assessments.ExpenseManagement.Infrastructure.Models;
using Assessments.ExpenseManagement.IntegrationTests.Abstractions;
using Assessments.ExpenseManagement.IntegrationTests.Fixtures;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace Assessments.ExpenseManagement.IntegrationTests.Infrastructure.Repositories;

[Collection(nameof(TestCollectionFixture))]
public sealed class CreateExpenseTests(TestContainerFactory factory) : TestBaseWithMigration(factory)
{
    [Fact]
    public async Task Should_ReturnExpense_When_Successful()
    {
        // Arrange
        var expected = Expense.Create(
            100M,
            CategoryPresets.Hotel,
            CurrencyPresets.USD,
            "Test comment",
            DateOnly.FromDateTime(DateTime.Now),
            UserPresets.JohnDoe);

        // Act
        var actual = await ExpenseWriteRepository.AddAsync(expected);
        var changes = await ExpenseWriteRepository.SaveChangesAsync();

        // Assert
        actual.ShouldBe(expected);
        changes.ShouldBe(1);

        var expenses = Context.Expenses.AsNoTracking();
        expenses.ShouldContain(expected);

        // Cleanup
        _ = Context.Expenses.Remove(expected);
        _ = await Context.SaveChangesAsync();
    }
}
