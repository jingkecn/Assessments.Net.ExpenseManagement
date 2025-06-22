using System.ComponentModel.DataAnnotations;
using Assessments.ExpenseManagement.Application.Models;
using Mediator;

namespace Assessments.ExpenseManagement.Application.Queries;

public sealed record GetEmployeeExpenseByIdQuery(
    [property: Required] Guid EmployeeId,
    [property: Required] Guid ExpenseId) : IQuery<ExpenseView>;
