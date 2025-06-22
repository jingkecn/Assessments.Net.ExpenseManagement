using System.ComponentModel.DataAnnotations;
using Assessments.ExpenseManagement.Application.Models;
using Mediator;

namespace Assessments.ExpenseManagement.Application.Queries;

public sealed record GetEmployeesQuery(
    [property: Required]
    [property: Range(1, int.MaxValue)]
    int PageNumber = 1,
    [property: Required]
    [property: Range(1, int.MaxValue)]
    int PageSize = 10) : IQuery<(IEnumerable<EmployeeView> Employees, int TotalCount)>;
