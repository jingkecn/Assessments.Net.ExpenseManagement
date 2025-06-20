using System.Runtime.CompilerServices;
using Assessments.ExpenseManagement.Domain.Models;

namespace Assessments.ExpenseManagement.Domain.Exceptions;

public sealed class DomainException : Exception
{
    private DomainException() { }
    private DomainException(string message) : base(message) { }

    public static DomainException CurrencyNotSupported(Currency currency) => new($"{currency} is not supported.");

    public static DomainException EmployeeNotAllowed(Guid employeeId, Category category) =>
        new($"Employee with ID {employeeId} is not allowed for {category}.");

    public static DomainException EmployeeNotAllowed(Guid employeeId, Currency currency) =>
        new($"Employee with ID {employeeId} is not allowed for {currency}.");

    public static DomainException InvalidAmount(Amount amount) =>
        new($"Invalid amount: {amount.Value}. Amount must be non-negative.");

    public static DomainException InvalidReceipts(IEnumerable<Receipt> receipts) =>
        new($"Invalid receipts: [{string.Join(", ", receipts.Select(r => r.FileUrl))}]. Receipts must be provided.");

    public static DomainException InvalidStatus(Status status, [CallerMemberName] string action = null!) =>
        new($"Invalid status: {status} for action: {action}.");

    public static DomainException NoReceipts() => new("Receipts must be provided.");
}
