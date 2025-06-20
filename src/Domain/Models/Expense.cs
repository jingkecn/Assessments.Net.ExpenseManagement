using System.ComponentModel.DataAnnotations;
using Assessments.ExpenseManagement.Domain.Abstractions;
using Assessments.ExpenseManagement.Domain.Contracts;
using Assessments.ExpenseManagement.Domain.Events;
using Assessments.ExpenseManagement.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Assessments.ExpenseManagement.Domain.Models;

[Index(nameof(Category), nameof(Currency), nameof(Employee), nameof(ExecutionDate))]
public sealed class Expense : Entity, IAggregateRoot
{
    private readonly List<Receipt> receipts = [];

    private Expense() { }

    [Required] public decimal Amount { get; private init; }
    [Required] public Guid CategoryId { get; private init; }
    public Category? Category { get; private init; }
    [Required] public Guid CurrencyId { get; private init; }
    public Currency? Currency { get; private init; }
    public Guid? EmployeeId { get; private init; }

    /// <summary>
    ///     Keeps track of historical employee information, in case the employee record is removed from the DB.
    /// </summary>
    [Required]
    public EmployeeInfo EmployeeInfo { get; private init; } = null!;

    public Employee? Employee { get; private init; }
    [Required] public DateOnly ExecutionDate { get; private init; }
    public IReadOnlyCollection<Receipt> Receipts => receipts.AsReadOnly();
    [Required] public Status Status { get; private set; }
    public DateOnly SubmissionDate { get; private set; }

    public static Expense Create(
        Employee employee,
        decimal amount,
        Category category,
        Currency currency,
        DateOnly executionDate,
        IEnumerable<Receipt>? receipts = null,
        bool isTransient = false) =>
        Create(isTransient ? Guid.Empty : Guid.NewGuid(), employee, amount, category, currency, executionDate,
            receipts);

    public static Expense Create(
        Guid id,
        Employee employee,
        decimal amount,
        Category category,
        Currency currency,
        DateOnly executionDate,
        IEnumerable<Receipt>? receipts = null)
    {
        var expense = new Expense
        {
            Id = id,
            Amount = amount,
            CategoryId = category.Id,
            CurrencyId = currency.Id,
            EmployeeInfo = new EmployeeInfo { FirstName = employee.FirstName, LastName = employee.LastName },
            EmployeeId = employee.Id,
            ExecutionDate = executionDate,
            Status = Status.Draft
        };

        expense.receipts.AddRange(receipts ?? []);
        return expense;
    }

    #region Business Logics

    public void AddReceipt(Receipt receipt)
    {
        receipts.Add(receipt);
        AddDomainEvent(new ReceiptAddedEvent(Id, receipt.Id));
    }

    public void RemoveReceipt(Receipt receipt)
    {
        receipts.Remove(receipt);
        AddDomainEvent(new ReceiptRemovedEvent(Id, receipt.Id));
    }

    public void Cancel()
    {
        if (EmployeeId is not { } employeeId)
        {
            // Removed employee should not perform any action.
            return;
        }

        if (Status is not Status.Draft)
        {
            throw DomainException.ExpenseInvalidStatusToPerformAction(Status);
        }

        Status = Status.Cancelled;
        AddDomainEvent(new ExpenseCancelledEvent(employeeId, Id));
    }

    public void Submit()
    {
        if (EmployeeId is not { } employeeId)
        {
            // Removed employee should not perform any action.
            return;
        }

        if (Status is not Status.Draft)
        {
            throw DomainException.ExpenseInvalidStatusToPerformAction(Status);
        }

        SubmissionDate = DateOnly.FromDateTime(DateTime.UtcNow);
        Status = Status.Submitted;
        AddDomainEvent(new ExpenseSubmittedEvent(employeeId, Id));
    }

    #endregion
}
