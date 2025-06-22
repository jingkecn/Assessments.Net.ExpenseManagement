using System.ComponentModel.DataAnnotations;
using Assessments.ExpenseManagement.Application.Models;

namespace Assessments.ExpenseManagement.Query.Api.Models;

public sealed record GetExpensesResponse(
    [property: Required] IEnumerable<ExpenseView> Expenses,
    [property: Required]
    [property: Range(1, int.MaxValue)]
    int PageNumber,
    [property: Required]
    [property: Range(1, int.MaxValue)]
    int PageSize,
    [property: Required]
    [property: Range(0, int.MaxValue)]
    int TotalCount);
