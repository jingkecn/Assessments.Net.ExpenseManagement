using System.ComponentModel.DataAnnotations;
using Mediator;

namespace Assessments.ExpenseManagement.Domain.Events;

public record ExpenseSubmittedEvent(
    [property: Required] Guid EmployeeId,
    [property: Required] Guid ExpenseId) : INotification;
