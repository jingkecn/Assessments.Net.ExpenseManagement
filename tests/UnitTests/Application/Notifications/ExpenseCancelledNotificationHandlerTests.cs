using Assessments.ExpenseManagement.Application.Extensions;
using Assessments.ExpenseManagement.Application.Notifications;
using Assessments.ExpenseManagement.Application.Specifications;
using Assessments.ExpenseManagement.Domain.Events;
using Assessments.ExpenseManagement.Domain.Models;
using Assessments.ExpenseManagement.Infrastructure.Presets;
using Assessments.ExpenseManagement.UnitTests.Abstractions;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Assessments.ExpenseManagement.UnitTests.Application.Notifications;

public sealed class ExpenseCancelledNotificationHandlerTests : TestBase
{
    private ILogger<ExpenseCancelledNotificationHandler> Logger { get; } =
        Substitute.For<ILogger<ExpenseCancelledNotificationHandler>>();

    [Fact]
    public async Task Handle_Should_LogExpenseCancelledEvent_When_Successful()
    {
        // Arrange
        var employee = EmployeePresets.JohnDoe;
        var expense = Expense.Create(
            employee,
            100M,
            CategoryPresets.Restaurant,
            CurrencyPresets.USD,
            DateOnly.FromDateTime(DateTime.Now),
            [Receipt.Create("No comment", "john.doe/receipt.jpg")]);
        var notification = new ExpenseCancelledEvent(employee.Id, expense.Id);
        var handler = new ExpenseCancelledNotificationHandler(ExpenseReadRepository, Logger);

        ExpenseReadRepository
            .ExistsAsync(Arg.Any<GetEmployeeExpenseByIdSpecification>())
            .Returns(true);
        ExpenseReadRepository
            .GetAsync(Arg.Any<GetEmployeeExpenseByIdSpecification>(), Arg.Any<int>(), Arg.Any<int>())
            .Returns([expense]);

        expense.Cancel(); // Change status to 'Cancelled'

        // Act
        await handler.Handle(notification, CancellationToken.None);

        // Assert
        await ExpenseReadRepository
            .Received(1)
            .ExistsAsync(Arg.Any<GetEmployeeExpenseByIdSpecification>());

        Logger
            .Received(1)
            .LogExpenseCancelled(employee.Id, expense.Id, Status.Cancelled);
    }

    [Fact]
    public async Task Handle_ShouldNot_LogExpenseCancelledEvent_When_ExpenseNotFound()
    {
        // Arrange
        var employee = EmployeePresets.JohnDoe;
        var expense = Expense.Create(
            employee,
            100M,
            CategoryPresets.Restaurant,
            CurrencyPresets.USD,
            DateOnly.FromDateTime(DateTime.Now),
            [Receipt.Create("No comment", "john.doe/receipt.jpg")]);
        var notification = new ExpenseCancelledEvent(employee.Id, expense.Id);
        var handler = new ExpenseCancelledNotificationHandler(ExpenseReadRepository, Logger);

        ExpenseReadRepository
            .ExistsAsync(Arg.Any<GetEmployeeExpenseByIdSpecification>())
            .Returns(false);

        // Act
        await handler.Handle(notification, CancellationToken.None);

        // Assert
        await ExpenseReadRepository
            .Received(1)
            .ExistsAsync(Arg.Any<GetEmployeeExpenseByIdSpecification>());

        Logger
            .Received(0)
            .LogExpenseCancelled(employee.Id, expense.Id, Status.Cancelled);
    }
}
