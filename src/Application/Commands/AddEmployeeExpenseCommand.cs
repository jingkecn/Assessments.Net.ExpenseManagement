using System.ComponentModel.DataAnnotations;
using Assessments.ExpenseManagement.Application.Models;
using Mediator;

namespace Assessments.ExpenseManagement.Application.Commands;

public sealed record AddEmployeeExpenseCommand(
    [property: Required] decimal Amount,
    [property: Required] string CategoryName,
    [property: Required] string CurrencyCode,
    [property: Required] Guid EmployeeId,
    [property: Required] DateOnly ExecutionDate,
    [property: Required] IEnumerable<ReceiptView> Receipts) : ICommand<Guid>;
