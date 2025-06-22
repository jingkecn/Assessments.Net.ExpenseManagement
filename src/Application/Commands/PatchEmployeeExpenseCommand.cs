using System.ComponentModel.DataAnnotations;
using Mediator;

namespace Assessments.ExpenseManagement.Application.Commands;

public record PatchEmployeeExpenseCommand(
    [property: Required] string Action,
    [property: Required] Guid EmployeeId,
    [property: Required] Guid ExpenseId) : ICommand<bool>;
