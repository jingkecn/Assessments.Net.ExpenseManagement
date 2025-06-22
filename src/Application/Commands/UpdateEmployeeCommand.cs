using System.ComponentModel.DataAnnotations;
using Mediator;

namespace Assessments.ExpenseManagement.Application.Commands;

public sealed record UpdateEmployeeCommand(
    [property: Required] Guid EmployeeId,
    [property: Required] string CurrencyCode) : ICommand<bool>;
