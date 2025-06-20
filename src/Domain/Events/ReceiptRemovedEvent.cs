using Mediator;

namespace Assessments.ExpenseManagement.Domain.Events;

public record ReceiptRemovedEvent(
    Guid ExpenseId,
    Guid ReceiptId) : INotification;
