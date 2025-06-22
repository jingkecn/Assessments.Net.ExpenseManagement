using Assessments.ExpenseManagement.Application.Constants;
using Assessments.ExpenseManagement.Domain.Models;
using Microsoft.Extensions.Logging;

namespace Assessments.ExpenseManagement.Application.Extensions;

internal static partial class ExpenseManagementTrace
{
    [LoggerMessage(EventId = 1, Level = LogLevel.Trace, Message = LoggerMessages.ExpenseCancelled)]
    public static partial void LogExpenseCancelled(
        this ILogger logger,
        Guid employeeId,
        Guid expenseId,
        Status expenseStatus);

    [LoggerMessage(EventId = 2, Level = LogLevel.Trace, Message = LoggerMessages.ExpenseSubmitted)]
    public static partial void LogExpenseSubmitted(
        this ILogger logger,
        Guid employeeId,
        Guid expenseId,
        Status expenseStatus);
}
