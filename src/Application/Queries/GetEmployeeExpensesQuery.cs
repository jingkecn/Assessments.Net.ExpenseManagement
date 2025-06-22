using System.ComponentModel.DataAnnotations;
using Assessments.ExpenseManagement.Application.Models;
using Mediator;

namespace Assessments.ExpenseManagement.Application.Queries;

public sealed record GetEmployeeExpensesQuery(
    [property: Required] Guid EmployeeId,
    [property: Required]
    [property: Range(1, int.MaxValue)]
    int PageNumber = 1,
    [property: Required]
    [property: Range(1, int.MaxValue)]
    int PageSize = 10) : IQuery<(IEnumerable<ExpenseView> Expenses, int TotalCount)>;
