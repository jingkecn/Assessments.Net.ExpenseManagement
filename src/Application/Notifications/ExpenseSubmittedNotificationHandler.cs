using Assessments.ExpenseManagement.Application.Extensions;
using Assessments.ExpenseManagement.Application.Specifications;
using Assessments.ExpenseManagement.Domain.Contracts;
using Assessments.ExpenseManagement.Domain.Events;
using Assessments.ExpenseManagement.Domain.Models;
using Mediator;
using Microsoft.Extensions.Logging;

namespace Assessments.ExpenseManagement.Application.Notifications;

public sealed class ExpenseSubmittedNotificationHandler(
    IReadRepository<Expense> expenseReadRepository,
    ILogger<ExpenseSubmittedNotificationHandler> logger) : INotificationHandler<ExpenseSubmittedEvent>
{
    public async ValueTask Handle(ExpenseSubmittedEvent notification, CancellationToken cancellationToken)
    {
        var (employeeId, expenseId) = notification;
        var specification = new GetEmployeeExpenseByIdSpecification(employeeId, expenseId);
        if (!await expenseReadRepository.ExistsAsync(specification, cancellationToken))
        {
            return;
        }

        var expenses = await expenseReadRepository.GetAsync(specification, cancellationToken: cancellationToken);
        var expense = expenses.Single();
        logger.LogExpenseSubmitted(expenseId, employeeId, expense.Status);
        // TODO: send message via MQ to notify the downstream services such as auditing service.
    }
}
