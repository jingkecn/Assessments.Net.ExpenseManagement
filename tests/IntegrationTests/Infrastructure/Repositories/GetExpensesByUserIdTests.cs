using Assessments.ExpenseManagement.Application.Specifications;
using Assessments.ExpenseManagement.Domain.Models;
using Assessments.ExpenseManagement.Infrastructure.Models;
using Assessments.ExpenseManagement.IntegrationTests.Abstractions;
using Assessments.ExpenseManagement.IntegrationTests.Fixtures;
using Shouldly;

namespace Assessments.ExpenseManagement.IntegrationTests.Infrastructure.Repositories;

[Collection(nameof(TestCollectionFixture))]
public sealed class GetExpensesByUserIdTests(TestContainerFactory factory) : TestBaseWithMigration(factory)
{
    private static IEnumerable<Expense> ExpensesOfJaneDoe
    {
        get
        {
            yield return Expense.Create(100M, CategoryPresets.Hotel, CurrencyPresets.EUR, "Test Hotel",
                DateOnly.FromDateTime(DateTime.Now), UserPresets.JaneDoe);
            yield return Expense.Create(200M, CategoryPresets.Restaurant, CurrencyPresets.EUR, "Test Restaurant",
                DateOnly.FromDateTime(DateTime.Now), UserPresets.JaneDoe);
            yield return Expense.Create(300M, CategoryPresets.Travel, CurrencyPresets.EUR, "Test Travel",
                DateOnly.FromDateTime(DateTime.Now), UserPresets.JaneDoe);
        }
    }

    private static IEnumerable<Expense> ExpensesOfJohnDoe
    {
        get
        {
            yield return Expense.Create(100M, CategoryPresets.Hotel, CurrencyPresets.USD, "Test Hotel",
                DateOnly.FromDateTime(DateTime.Now), UserPresets.JohnDoe);
            yield return Expense.Create(200M, CategoryPresets.Restaurant, CurrencyPresets.USD, "Test Restaurant",
                DateOnly.FromDateTime(DateTime.Now), UserPresets.JohnDoe);
            yield return Expense.Create(300M, CategoryPresets.Travel, CurrencyPresets.USD, "Test Travel",
                DateOnly.FromDateTime(DateTime.Now), UserPresets.JohnDoe);
        }
    }

    [Fact]
    public async Task Should_ReturnExpensesByUserIdOrderByAmount_When_Successful_With_SortByAmount()
    {
        // Arrange
        var expensesOfJohnDoe = ExpensesOfJohnDoe.ToList();
        await Task.WhenAll(expensesOfJohnDoe.Select(e => ExpenseWriteRepository.AddAsync(e)));
        var expensesOfJaneDoe = ExpensesOfJaneDoe.ToList();
        await Task.WhenAll(expensesOfJaneDoe.Select(e => ExpenseWriteRepository.AddAsync(e)));
        _ = await ExpenseWriteRepository.SaveChangesAsync();

        var user = UserPresets.JohnDoe;
        var specification = new GetExpensesByUserIdOrderByAmountAscendingSpecification(user.Id);

        // Act
        var actual = await ExpenseReadRepository.GetAsync(specification);

        // Assert
        var expected = expensesOfJohnDoe.OrderBy(e => e.Amount);
        actual.ShouldBe(expected);

        // Cleanup
        await Task.WhenAll(expensesOfJohnDoe.Select(e => ExpenseWriteRepository.DeleteAsync(e)));
        await Task.WhenAll(expensesOfJaneDoe.Select(e => ExpenseWriteRepository.DeleteAsync(e)));
        _ = await ExpenseWriteRepository.SaveChangesAsync();
    }

    [Fact]
    public async Task Should_ReturnExpensesByUserIdOrderByExecutionDate_When_Successful_With_SortByExecutionDate()
    {
        // Arrange
        var expensesOfJohnDoe = ExpensesOfJohnDoe.ToList();
        await Task.WhenAll(expensesOfJohnDoe.Select(e => ExpenseWriteRepository.AddAsync(e)));
        var expensesOfJaneDoe = ExpensesOfJaneDoe.ToList();
        await Task.WhenAll(expensesOfJaneDoe.Select(e => ExpenseWriteRepository.AddAsync(e)));
        _ = await ExpenseWriteRepository.SaveChangesAsync();

        var user = UserPresets.JohnDoe;
        var specification = new GetExpensesByUserIdOrderByExecutionDateDescendingSpecification(user.Id);

        // Act
        var actual = await ExpenseReadRepository.GetAsync(specification);

        // Assert
        var expected = expensesOfJohnDoe.OrderByDescending(e => e.ExecutionDate);
        actual.ShouldBe(expected);

        // Cleanup
        await Task.WhenAll(expensesOfJohnDoe.Select(e => ExpenseWriteRepository.DeleteAsync(e)));
        await Task.WhenAll(expensesOfJaneDoe.Select(e => ExpenseWriteRepository.DeleteAsync(e)));
        _ = await ExpenseWriteRepository.SaveChangesAsync();
    }
}
