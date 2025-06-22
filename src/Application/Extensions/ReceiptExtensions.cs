using Assessments.ExpenseManagement.Application.Models;
using Assessments.ExpenseManagement.Domain.Models;

namespace Assessments.ExpenseManagement.Application.Extensions;

public static class ReceiptExtensions
{
    public static ReceiptView ToView(this Receipt source) => new(source.Comment, source.FileUrl);

    public static IEnumerable<ReceiptView> ToView(this IEnumerable<Receipt> source) => source.Select(r => r.ToView());
}
