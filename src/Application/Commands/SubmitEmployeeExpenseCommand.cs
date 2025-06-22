using System.ComponentModel.DataAnnotations;
using Mediator;

namespace Assessments.ExpenseManagement.Application.Commands;

public sealed record SubmitEmployeeExpenseCommand(
    [property: Required] Guid EmployeeId,
    [property: Required] Guid ExpenseId) : ICommand<bool>;
