using Assessments.ExpenseManagement.Application.Extensions;
using Assessments.ExpenseManagement.Application.Queries;
using Assessments.ExpenseManagement.Application.Specifications;
using Assessments.ExpenseManagement.Application.Validations;
using Assessments.ExpenseManagement.Domain.Models;
using Assessments.ExpenseManagement.Infrastructure.Presets;
using Assessments.ExpenseManagement.UnitTests.Abstractions;
using FluentValidation;
using NSubstitute;
using Shouldly;

namespace Assessments.ExpenseManagement.UnitTests.Application.Queries;

public sealed class GetExpensesQueryHandlerTests : TestBase
{
    private static IEnumerable<Expense> Expenses
    {
        get
        {
            yield return Expense.Create(
                EmployeePresets.JaneDoe,
                100M,
                CategoryPresets.Hotel,
                CurrencyPresets.EUR,
                DateOnly.FromDateTime(DateTime.Now),
                [Receipt.Create("No comment", "jane.doe/receipt.jpg")]);
            yield return Expense.Create(
                EmployeePresets.JohnDoe,
                100M,
                CategoryPresets.Restaurant,
                CurrencyPresets.USD,
                DateOnly.FromDateTime(DateTime.Now),
                [Receipt.Create("No comment", "john.doe/receipt.jpg")]);
        }
    }

    private IValidator<GetExpensesQuery> Validator { get; } = new GetExpensesQueryValidator();

    [Fact]
    public async Task Handle_Should_GetPaginatedExpenses_When_Successful()
    {
        // Arrange
        var query = new GetExpensesQuery(1, 1);
        var handler = new GetExpensesQueryHandler(ExpenseReadRepository, Validator);

        var expected = Expenses.ToList();
        ExpenseReadRepository
            .CountAsync(Arg.Any<GetExpensesSpecification>())
            .Returns(expected.Count);

        var expectedWithPagination = expected
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToList();
        ExpenseReadRepository
            .GetAsync(Arg.Any<GetExpensesSpecification>(), query.PageNumber, query.PageSize)
            .Returns(expectedWithPagination);

        // Act
        var (expenses, totalCount) = await handler.Handle(query, CancellationToken.None);

        // Assert
        var actual = expenses.ToList();
        var views = expectedWithPagination.ToView().ToList();
        actual.Select(e => e.Amount).ShouldBe(views.Select(e => e.Amount), true);
        actual.Select(e => e.Employee).ShouldBe(views.Select(e => e.Employee), true);
        actual.Select(e => e.ExecutionDate).ShouldBe(views.Select(e => e.ExecutionDate), true);
        actual.Select(e => e.Status).ShouldBe(views.Select(e => e.Status), true);
        actual.Select(e => e.SubmissionDate).ShouldBe(views.Select(e => e.SubmissionDate), true);
        totalCount.ShouldBe(expected.Count);

        await ExpenseReadRepository
            .Received(1)
            .CountAsync(Arg.Any<GetExpensesSpecification>());
        await ExpenseReadRepository
            .Received(1)
            .GetAsync(Arg.Any<GetExpensesSpecification>(), query.PageNumber, query.PageSize);
    }
}
