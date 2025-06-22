using Assessments.ExpenseManagement.Domain.Models;

namespace Assessments.ExpenseManagement.Application.Constants;

public static class LoggerMessages
{
    public const string ExpenseCancelled =
        $"{{expenseStatus}} {nameof(Expense)} [Id={{expenseId}}] for {nameof(Employee)} [Id={{employeeId}}].";

    public const string ExpenseSubmitted =
        $"{{expenseStatus}} {nameof(Expense)} [Id={{expenseId}}] for {nameof(Employee)} [Id={{employeeId}}].";
}
