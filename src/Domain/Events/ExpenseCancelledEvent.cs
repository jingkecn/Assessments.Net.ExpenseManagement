using System.ComponentModel.DataAnnotations;
using Mediator;

namespace Assessments.ExpenseManagement.Domain.Events;

public sealed record ExpenseCancelledEvent(
    [property: Required] Guid EmployeeId,
    [property: Required] Guid ExpenseId) : INotification;
