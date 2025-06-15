using Assessments.ExpenseManagement.Application.Models;
using Assessments.ExpenseManagement.Domain.Models;

namespace Assessments.ExpenseManagement.Application.Extensions;

public static class ExpenseExtensions
{
    public static ExpenseView ToView(this Expense source) => new(
        source.Id,
        source.Amount,
        source.Category,
        source.Currency,
        source.Description,
        source.ExecutionDate,
        source.Status,
        source.SubmissionDate,
        source.User);

    public static IEnumerable<ExpenseView> ToView(this IEnumerable<Expense> source) => source.Select(ToView);
}
