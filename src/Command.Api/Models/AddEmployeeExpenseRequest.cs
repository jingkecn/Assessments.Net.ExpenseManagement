using System.ComponentModel.DataAnnotations;
using Assessments.ExpenseManagement.Application.Models;

namespace Assessments.ExpenseManagement.Command.Api.Models;

public sealed record AddEmployeeExpenseRequest(
    [property: Required] decimal Amount,
    [property: Required] string CategoryName,
    [property: Required]
    [property: MaxLength(3)]
    [property: MinLength(3)]
    string CurrencyCode,
    [property: Required] DateOnly ExecutionDate,
    [property: Required] IEnumerable<ReceiptView> Receipts);
