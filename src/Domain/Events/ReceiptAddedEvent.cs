using Mediator;

namespace Assessments.ExpenseManagement.Domain.Events;

public record ReceiptAddedEvent(
    Guid ExpenseId,
    Guid ReceiptId) : INotification;
