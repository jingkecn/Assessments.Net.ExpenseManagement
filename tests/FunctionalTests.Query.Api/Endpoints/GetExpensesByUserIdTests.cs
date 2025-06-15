using System.Net;
using System.Net.Http.Json;
using Assessments.ExpenseManagement.Application.Extensions;
using Assessments.ExpenseManagement.Application.Models;
using Assessments.ExpenseManagement.Application.UseCases.Get;
using Assessments.ExpenseManagement.Domain.Models;
using Assessments.ExpenseManagement.FunctionalTests.Query.Api.Abstractions;
using Assessments.ExpenseManagement.FunctionalTests.Query.Api.Fixtures;
using Assessments.ExpenseManagement.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using SortDirection = Assessments.ExpenseManagement.Application.Models.SortDirection;

namespace Assessments.ExpenseManagement.FunctionalTests.Query.Api.Endpoints;

public sealed class GetExpensesByUserIdTests(TestWebApplicationFactory factory) : TestBase(factory)
{
    private static IEnumerable<Expense> ExpensesOfJaneDoe
    {
        get
        {
            yield return Expense.Create(100.00M, CategoryPresets.Hotel, CurrencyPresets.EUR, "Test Hotel",
                DateOnly.FromDateTime(DateTime.Now.AddDays(-2)), UserPresets.JaneDoe);
            yield return Expense.Create(200.00M, CategoryPresets.Restaurant, CurrencyPresets.EUR, "Test Restaurant",
                DateOnly.FromDateTime(DateTime.Now.AddDays(-1)), UserPresets.JaneDoe);
            yield return Expense.Create(300.00M, CategoryPresets.Travel, CurrencyPresets.EUR, "Test Travel",
                DateOnly.FromDateTime(DateTime.Now), UserPresets.JaneDoe);
        }
    }

    private static IEnumerable<Expense> ExpensesOfJohnDoe
    {
        get
        {
            yield return Expense.Create(100.00M, CategoryPresets.Hotel, CurrencyPresets.USD, "Test Hotel",
                DateOnly.FromDateTime(DateTime.Now), UserPresets.JohnDoe);
            yield return Expense.Create(200.00M, CategoryPresets.Restaurant, CurrencyPresets.USD, "Test Restaurant",
                DateOnly.FromDateTime(DateTime.Now), UserPresets.JohnDoe);
            yield return Expense.Create(300.00M, CategoryPresets.Travel, CurrencyPresets.USD, "Test Travel",
                DateOnly.FromDateTime(DateTime.Now), UserPresets.JohnDoe);
        }
    }

    [Fact]
    public async Task Should_ReturnOk_When_Successful_With_SortByAmountAscending()
    {
        // Arrange
        var expensesOfJaneDoe = ExpensesOfJaneDoe.ToList();
        await Context.Expenses.AddRangeAsync(expensesOfJaneDoe);
        var expensesOfJohnDoe = ExpensesOfJohnDoe.ToList();
        await Context.Expenses.AddRangeAsync(expensesOfJohnDoe);
        _ = await Context.SaveChangesAsync();

        var (userId, sortBy, sortDirection) =
            new GetExpensesByUserIdQuery(UserPresets.JohnDoe.Id, SortBy.Amount, SortDirection.Ascending);

        // Act
        var response = await HttpClient.GetAsync(
            $"api/expenses?{nameof(userId)}={userId}&{nameof(sortBy)}={sortBy}&{nameof(sortDirection)}={sortDirection}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var content = await response.Content.ReadFromJsonAsync<TestResponse>();
        var actual = content.ShouldNotBeNull().Expenses.ToList();
        var expenses = await Context.Expenses.AsNoTracking()
            .Where(e => e.UserId == userId)
            .OrderBy(e => e.Amount)
            .Include(e => e.Category)
            .Include(e => e.Currency)
            .Include(e => e.User)
            .ToListAsync();
        var expected = expenses.ToView().ToList();
        actual.Select(e => e.Id).ShouldBe(expected.Select(e => e.Id));

        // Cleanup
        Context.Expenses.RemoveRange(expensesOfJohnDoe);
        Context.Expenses.RemoveRange(expensesOfJaneDoe);
        _ = await Context.SaveChangesAsync();
    }

    [Fact]
    public async Task Should_ReturnOk_When_Successful_With_SortByAmountDescending()
    {
        // Arrange
        var expensesOfJohnDoe = ExpensesOfJohnDoe.ToList();
        await Context.Expenses.AddRangeAsync(expensesOfJohnDoe);
        var expensesOfJaneDoe = ExpensesOfJaneDoe.ToList();
        await Context.Expenses.AddRangeAsync(expensesOfJaneDoe);
        _ = await Context.SaveChangesAsync();

        var (userId, sortBy, sortDirection) =
            new GetExpensesByUserIdQuery(UserPresets.JohnDoe.Id, SortBy.Amount, SortDirection.Descending);

        // Act
        var response = await HttpClient.GetAsync(
            $"api/expenses?{nameof(userId)}={userId}&{nameof(sortBy)}={sortBy}&{nameof(sortDirection)}={sortDirection}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var content = await response.Content.ReadFromJsonAsync<TestResponse>();
        var actual = content.ShouldNotBeNull().Expenses.ToList();
        var expenses = await Context.Expenses.AsNoTracking()
            .Where(e => e.UserId == userId)
            .OrderByDescending(e => e.Amount)
            .Include(e => e.Category)
            .Include(e => e.Currency)
            .Include(e => e.User)
            .ToListAsync();
        var expected = expenses.ToView().ToList();
        actual.Select(e => e.Id).ShouldBe(expected.Select(e => e.Id));

        // Cleanup
        Context.Expenses.RemoveRange(expensesOfJohnDoe);
        Context.Expenses.RemoveRange(expensesOfJaneDoe);
        _ = await Context.SaveChangesAsync();
    }

    [Fact]
    public async Task Should_ReturnOk_When_Successful_With_SortByExecutionDateAscending()
    {
        // Arrange
        var expensesOfJohnDoe = ExpensesOfJohnDoe.ToList();
        await Context.Expenses.AddRangeAsync(expensesOfJohnDoe);
        var expensesOfJaneDoe = ExpensesOfJaneDoe.ToList();
        await Context.Expenses.AddRangeAsync(expensesOfJaneDoe);
        _ = await Context.SaveChangesAsync();

        var (userId, sortBy, sortDirection) =
            new GetExpensesByUserIdQuery(UserPresets.JaneDoe.Id, SortBy.ExecutionDate, SortDirection.Ascending);

        // Act
        var response = await HttpClient.GetAsync(
            $"api/expenses?{nameof(userId)}={userId}&{nameof(sortBy)}={sortBy}&{nameof(sortDirection)}={sortDirection}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var content = await response.Content.ReadFromJsonAsync<TestResponse>();
        var actual = content.ShouldNotBeNull().Expenses.ToList();
        var expenses = await Context.Expenses.AsNoTracking()
            .Where(e => e.UserId == userId)
            .OrderBy(e => e.ExecutionDate)
            .Include(e => e.Category)
            .Include(e => e.Currency)
            .Include(e => e.User)
            .ToListAsync();
        var expected = expenses.ToView().ToList();
        actual.Select(e => e.Id).ShouldBe(expected.Select(e => e.Id));

        // Cleanup
        Context.Expenses.RemoveRange(expensesOfJohnDoe);
        Context.Expenses.RemoveRange(expensesOfJaneDoe);
        _ = await Context.SaveChangesAsync();
    }

    [Fact]
    public async Task Should_ReturnOk_When_Successful_With_SortByExecutionDateDescending()
    {
        // Arrange
        var expensesOfJohnDoe = ExpensesOfJohnDoe.ToList();
        await Context.Expenses.AddRangeAsync(expensesOfJohnDoe);
        var expensesOfJaneDoe = ExpensesOfJaneDoe.ToList();
        await Context.Expenses.AddRangeAsync(expensesOfJaneDoe);
        _ = await Context.SaveChangesAsync();

        var (userId, sortBy, sortDirection) = new GetExpensesByUserIdQuery(
            UserPresets.JaneDoe.Id, SortBy.ExecutionDate, SortDirection.Descending);

        // Act
        var response = await HttpClient.GetAsync(
            $"api/expenses?{nameof(userId)}={userId}&{nameof(sortBy)}={sortBy}&{nameof(sortDirection)}={sortDirection}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var content = await response.Content.ReadFromJsonAsync<TestResponse>();
        var actual = content.ShouldNotBeNull().Expenses.ToList();
        var expenses = await Context.Expenses.AsNoTracking()
            .Where(e => e.UserId == userId)
            .OrderByDescending(e => e.ExecutionDate)
            .Include(e => e.Category)
            .Include(e => e.Currency)
            .Include(e => e.User)
            .ToListAsync();
        var expected = expenses.ToView().ToList();
        actual.Select(e => e.Id).ShouldBe(expected.Select(e => e.Id));

        // Cleanup
        Context.Expenses.RemoveRange(expensesOfJohnDoe);
        Context.Expenses.RemoveRange(expensesOfJaneDoe);
        _ = await Context.SaveChangesAsync();
    }

    private sealed record TestResponse
    {
        public IEnumerable<ExpenseView> Expenses { get; init; } = null!;
    }
}
