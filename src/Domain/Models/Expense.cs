using System.ComponentModel.DataAnnotations;
using Assessments.ExpenseManagement.Domain.Abstractions;
using Assessments.ExpenseManagement.Domain.Contracts;
using Assessments.ExpenseManagement.Domain.Events;
using Assessments.ExpenseManagement.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Assessments.ExpenseManagement.Domain.Models;

[Index(nameof(Amount), nameof(Category), nameof(Currency), nameof(Employee), nameof(SubmissionDate), IsUnique = true)]
public sealed class Expense : Entity, IAggregateRoot
{
    private readonly List<Receipt> receipts = [];

    private Expense() { }

    [Required] public Amount Amount { get; private init; }
    [Required] public Category Category { get; private init; }
    [Required] public Currency Currency { get; private init; }
    [Required] public Employee Employee { get; private init; }
    public DateOnly SubmissionDate { get; private set; }
    public IReadOnlyCollection<Receipt> Receipts => receipts.AsReadOnly();
    [Required] public Status Status { get; private set; }

    public static Expense Create(
        Employee employee,
        Amount amount,
        Category category,
        Currency currency,
        IEnumerable<Receipt>? receipts = null,
        bool isTransient = false)
    {
        var expense = new Expense
        {
            Id = isTransient ? Guid.Empty : Guid.NewGuid(),
            Amount = amount,
            Category = category,
            Currency = currency,
            Employee = employee,
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

    public void Submit()
    {
        Validate();
        if (Status != Status.Draft)
        {
            throw DomainException.InvalidStatus(Status);
        }

        SubmissionDate = DateOnly.FromDateTime(DateTime.UtcNow);
        Status = Status.Submitted;
        AddDomainEvent(new ExpenseSubmittedEvent(Id, Employee.Id));
    }

    public void Validate()
    {
        if (!Amount.IsValid)
        {
            throw DomainException.InvalidAmount(Amount);
        }

        if (!Category.IsAllowedFor(Employee))
        {
            throw DomainException.EmployeeNotAllowed(Employee.Id, Category);
        }

        if (!Currency.IsAllowedFor(Employee))
        {
            throw DomainException.EmployeeNotAllowed(Employee.Id, Currency);
        }

        // if (!Currency.IsSupported)
        // {
        //     throw DomainException.CurrencyNotSupported(Currency);
        // }

        if (Receipts.Count is 0)
        {
            throw DomainException.NoReceipts();
        }

        var invalidReceipts = Receipts.Where(r => !r.Exists).ToList();
        if (invalidReceipts.Count is not 0)
        {
            throw DomainException.InvalidReceipts(invalidReceipts);
        }
    }

    #endregion
}
