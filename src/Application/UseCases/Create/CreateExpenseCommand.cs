using System.ComponentModel.DataAnnotations;
using Mediator;

namespace Assessments.ExpenseManagement.Application.UseCases.Create;

public sealed record CreateExpenseCommand(
    [property: Required] decimal Amount,
    [property: Required] string CategoryName,
    [property: Required] string CurrencyCode,
    [property: Required]
    [property: MaxLength(128)]
    string Description,
    [property: Required] DateOnly ExecutionDate,
    [property: Required] Guid UserId) : ICommand<Guid>;
