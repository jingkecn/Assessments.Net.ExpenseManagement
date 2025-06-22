using Assessments.ExpenseManagement.Application.Models;
using Assessments.ExpenseManagement.Domain.Models;

namespace Assessments.ExpenseManagement.Application.Extensions;

public static class ExpenseExtensions
{
    public static ExpenseView ToView(this Expense source) => new(
        source.Amount,
        source.Currency is null ? null : $"{source.Currency.Symbol}{source.Amount}",
        source.Category?.Name,
        source.Currency?.Code,
        $"{source.EmployeeInfo.FirstName} {source.EmployeeInfo.LastName}",
        source.ExecutionDate,
        source.Receipts.Select(r => r.ToView()),
        source.Status.ToString(),
        source.SubmissionDate);

    public static IEnumerable<ExpenseView> ToView(this IEnumerable<Expense> source) => source.Select(e => e.ToView());
}
