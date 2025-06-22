using System.ComponentModel.DataAnnotations;
using Assessments.ExpenseManagement.Application.Models;
using Mediator;

namespace Assessments.ExpenseManagement.Application.Queries;

public sealed record GetExpenseByIdQuery([property: Required] Guid Id) : IQuery<ExpenseView>;
