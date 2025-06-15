using System.ComponentModel.DataAnnotations;

namespace Assessments.ExpenseManagement.Command.Api.Models;

public record CreateExpenseRequest(
    [property: Required] decimal Amount,
    [property: Required] string CategoryName,
    [property: Required] string CurrencyCode,
    [property: Required]
    [property: MaxLength(128)]
    string Description,
    [property: Required] DateOnly ExecutionDate,
    [property: Required] Guid UserId);
