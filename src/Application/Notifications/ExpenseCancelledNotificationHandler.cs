using Assessments.ExpenseManagement.Application.Extensions;
using Assessments.ExpenseManagement.Application.Specifications;
using Assessments.ExpenseManagement.Domain.Contracts;
using Assessments.ExpenseManagement.Domain.Events;
using Assessments.ExpenseManagement.Domain.Models;
using Mediator;
using Microsoft.Extensions.Logging;

namespace Assessments.ExpenseManagement.Application.Notifications;

public sealed class ExpenseCancelledNotificationHandler(
    IReadRepository<Expense> expenseReadRepository,
    ILogger<ExpenseCancelledNotificationHandler> logger) : INotificationHandler<ExpenseCancelledEvent>
{
    public async ValueTask Handle(ExpenseCancelledEvent notification, CancellationToken cancellationToken)
    {
        var (employeeId, expenseId) = notification;
        var specification = new GetEmployeeExpenseByIdSpecification(employeeId, expenseId);
        if (!await expenseReadRepository.ExistsAsync(specification, cancellationToken))
        {
            return;
        }

        var results = await expenseReadRepository.GetAsync(specification, cancellationToken: cancellationToken);
        var expense = results.Single();
        logger.LogExpenseCancelled(employeeId, expenseId, expense.Status);
        // TODO: send message via MQ to notify the downstream services.
    }
}
