using Assessments.ExpenseManagement.Application.Extensions;
using Assessments.ExpenseManagement.Application.Queries;
using Assessments.ExpenseManagement.Application.Specifications;
using Assessments.ExpenseManagement.Application.Validations;
using Assessments.ExpenseManagement.Domain.Exceptions;
using Assessments.ExpenseManagement.Domain.Models;
using Assessments.ExpenseManagement.Infrastructure.Presets;
using Assessments.ExpenseManagement.UnitTests.Abstractions;
using FluentValidation;
using NSubstitute;
using Shouldly;

namespace Assessments.ExpenseManagement.UnitTests.Application.Queries;

public sealed class GetEmployeeExpensesQueryHandlerTests : TestBase
{
    private static readonly Employee Employee = EmployeePresets.JohnDoe;

    private static IEnumerable<Expense> EmployeeExpenses
    {
        get
        {
            yield return Expense.Create(
                Employee,
                100M,
                CategoryPresets.Hotel,
                CurrencyPresets.USD,
                DateOnly.FromDateTime(DateTime.Now),
                [Receipt.Create("No comment", "john.doe/receipt_1.jpg")]);
            yield return Expense.Create(
                Employee,
                100M,
                CategoryPresets.Restaurant,
                CurrencyPresets.USD,
                DateOnly.FromDateTime(DateTime.Now),
                [Receipt.Create("No comment", "john.doe/receipt_2.jpg")]);
        }
    }

    private IValidator<GetEmployeeExpensesQuery> Validator { get; } = new GetEmployeeExpensesQueryValidator();

    [Fact]
    public async Task Handle_Should_GetPaginatedEmployeeExpenses_When_Successful()
    {
        // Arrange
        var query = new GetEmployeeExpensesQuery(Employee.Id, 1, 1);
        var handler = new GetEmployeeExpensesQueryHandler(EmployeeReadRepository, ExpenseReadRepository, Validator);

        EmployeeReadRepository
            .ExistsAsync(Arg.Any<GetEmployeeByIdSpecification>())
            .Returns(true);

        var expected = EmployeeExpenses.ToList();
        ExpenseReadRepository
            .CountAsync(Arg.Any<GetEmployeeExpensesSpecification>())
            .Returns(expected.Count);

        var expectedWithPagination = expected
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToList();
        ExpenseReadRepository
            .GetAsync(Arg.Any<GetEmployeeExpensesSpecification>(), query.PageNumber, query.PageSize)
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
            .CountAsync(Arg.Any<GetEmployeeExpensesSpecification>());
        await ExpenseReadRepository
            .Received(1)
            .GetAsync(Arg.Any<GetEmployeeExpensesSpecification>(), query.PageNumber, query.PageSize);
    }

    [Fact]
    public async Task Handle_Should_ThrowDomainException_When_EmployeeNotFound()
    {
        // Arrange
        var employeeId = Guid.NewGuid();
        var query = new GetEmployeeExpensesQuery(employeeId, 1, 1);
        var handler = new GetEmployeeExpensesQueryHandler(EmployeeReadRepository, ExpenseReadRepository, Validator);

        // Act
        var action = async () => await handler.Handle(query, CancellationToken.None);

        // Assert
        var exception = await action.ShouldThrowAsync<DomainException>();
        var expected = DomainException.EmployeeNotFoundById(employeeId);
        exception.StatusCode.ShouldBe(expected.StatusCode);
        exception.Message.ShouldBe(expected.Message);

        await ExpenseReadRepository
            .Received(0)
            .CountAsync(Arg.Any<GetEmployeeExpensesSpecification>());
        await ExpenseReadRepository
            .Received(0)
            .GetAsync(Arg.Any<GetEmployeeExpensesSpecification>(), query.PageNumber, query.PageSize);
    }
}
