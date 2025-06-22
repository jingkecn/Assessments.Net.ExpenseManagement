using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Assessments.ExpenseManagement.Application.Models;

public sealed record ExpenseView(
    [property: Required] decimal Amount,
    [property: Required]
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    string? AmountWithCurrency,
    [property: Required]
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    string? Category,
    [property: Required]
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [property: MaxLength(3)]
    [property: MinLength(3)]
    string? Currency,
    [property: Required] string Employee,
    [property: Required] DateOnly ExecutionDate,
    [property: Required] IEnumerable<ReceiptView> Receipts,
    [property: Required] string Status,
    [property: Required] DateOnly SubmissionDate);
