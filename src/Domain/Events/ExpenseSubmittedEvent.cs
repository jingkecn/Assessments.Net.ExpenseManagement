using Mediator;

namespace Assessments.ExpenseManagement.Domain.Events;

public record ExpenseSubmittedEvent(
    Guid ExpenseId,
    Guid EmployeeId) : INotification;
