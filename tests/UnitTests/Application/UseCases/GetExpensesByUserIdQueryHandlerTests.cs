using Assessments.ExpenseManagement.Application.Extensions;
using Assessments.ExpenseManagement.Application.Models;
using Assessments.ExpenseManagement.Application.Specifications;
using Assessments.ExpenseManagement.Application.UseCases.Get;
using Assessments.ExpenseManagement.Domain.Models;
using Assessments.ExpenseManagement.Infrastructure.Models;
using Assessments.ExpenseManagement.UnitTests.Abstractions;
using NSubstitute;
using Shouldly;
using SortDirection = Assessments.ExpenseManagement.Application.Models.SortDirection;

namespace Assessments.ExpenseManagement.UnitTests.Application.UseCases;

public sealed class GetExpensesByUserIdQueryHandlerTests : TestBase
{
    [Fact]
    public async Task Handle_Should_ReturnExpenseOrderedByAmount_When_QueryWithSortByAmount()
    {
        // Arrange
        var category = CategoryPresets.Hotel;
        var currency = CurrencyPresets.USD;
        var user = UserPresets.JohnDoe;
        user.Currency = currency;
        var expenses = new List<Expense>
        {
            Expense.Create(
                100M,
                category,
                currency,
                "Test comment",
                DateOnly.FromDateTime(DateTime.Now).AddDays(-1),
                user),
            Expense.Create(
                200M,
                category,
                currency,
                "Test comment",
                DateOnly.FromDateTime(DateTime.Now),
                user)
        };
        expenses.ForEach(e =>
        {
            e.Category = category;
            e.Currency = currency;
            e.User = user;
        });
        var query = new GetExpensesByUserIdQuery(user.Id, SortBy.Amount, SortDirection.Ascending);
        var handler = new GetExpensesByUserIdQueryHandler(ExpenseReadRepository);

        ExpenseReadRepository
            .GetAsync(Arg.Any<GetExpensesByUserIdOrderByAmountAscendingSpecification>())
            .Returns(expenses.OrderBy(e => e.Amount));

        // Act
        var actual = await handler.Handle(query, CancellationToken.None);

        // Assert
        var expected = expenses.Select(e => e.ToView());
        actual.ShouldBe(expected);
    }

    [Fact]
    public async Task Handle_Should_ReturnExpenseOrderedByAmount_When_QueryWithSortByExecutionDate()
    {
        // Arrange
        var category = CategoryPresets.Hotel;
        var currency = CurrencyPresets.USD;
        var user = UserPresets.JohnDoe;
        user.Currency = currency;
        var expenses = new List<Expense>
        {
            Expense.Create(
                100M,
                category,
                currency,
                "Test comment",
                DateOnly.FromDateTime(DateTime.Now).AddDays(-1),
                user),
            Expense.Create(
                200M,
                category,
                currency,
                "Test comment",
                DateOnly.FromDateTime(DateTime.Now),
                user)
        };
        expenses.ForEach(e =>
        {
            e.Category = category;
            e.Currency = currency;
            e.User = user;
        });
        var query = new GetExpensesByUserIdQuery(user.Id, SortBy.ExecutionDate, SortDirection.Descending);
        var handler = new GetExpensesByUserIdQueryHandler(ExpenseReadRepository);

        ExpenseReadRepository
            .GetAsync(Arg.Any<GetExpensesByUserIdOrderByExecutionDateDescendingSpecification>())
            .Returns(expenses.OrderByDescending(e => e.ExecutionDate));

        // Act
        var actual = await handler.Handle(query, CancellationToken.None);

        // Assert
        var expected = expenses.Select(e => e.ToView()).OrderByDescending(e => e.ExecutionDate);
        actual.ShouldBe(expected);
    }
}
