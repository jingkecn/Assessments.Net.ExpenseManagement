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

public sealed class ExpenseSubmittedNotificationHandlerTests : TestBase
{
    private ILogger<ExpenseSubmittedNotificationHandler> Logger { get; } =
        Substitute.For<ILogger<ExpenseSubmittedNotificationHandler>>();

    [Fact]
    public async Task Handle_Should_LogExpenseSubmittedEvent_When_Successful()
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
        var notification = new ExpenseSubmittedEvent(employee.Id, expense.Id);
        var handler = new ExpenseSubmittedNotificationHandler(ExpenseReadRepository, Logger);

        ExpenseReadRepository
            .ExistsAsync(Arg.Any<GetEmployeeExpenseByIdSpecification>())
            .Returns(true);
        ExpenseReadRepository
            .GetAsync(Arg.Any<GetEmployeeExpenseByIdSpecification>(), Arg.Any<int>(), Arg.Any<int>())
            .Returns([expense]);

        expense.Cancel(); // Change status to 'Submitted'

        // Act
        await handler.Handle(notification, CancellationToken.None);

        // Assert
        await ExpenseReadRepository
            .Received(1)
            .ExistsAsync(Arg.Any<GetEmployeeExpenseByIdSpecification>());

        Logger
            .Received(1)
            .LogExpenseSubmitted(employee.Id, expense.Id, Status.Submitted);
    }

    [Fact]
    public async Task Handle_ShouldNot_LogExpenseSubmittedEvent_When_ExpenseNotFound()
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
        var notification = new ExpenseSubmittedEvent(employee.Id, expense.Id);
        var handler = new ExpenseSubmittedNotificationHandler(ExpenseReadRepository, Logger);

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
            .LogExpenseSubmitted(employee.Id, expense.Id, Status.Submitted);
    }
}
