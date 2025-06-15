using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Assessments.ExpenseManagement.Domain.Abstractions;
using Assessments.ExpenseManagement.Domain.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Assessments.ExpenseManagement.Domain.Models;

[Index(nameof(Amount), nameof(ExecutionDate), nameof(UserId), IsUnique = true)]
public sealed class Expense : Entity, IAggregateRoot
{
    private Expense() { }

    [Required] public decimal Amount { get; private init; }
    [Required] public Guid CategoryId { get; private init; }
    public Category? Category { get; internal set; }
    [Required] public Guid CurrencyId { get; private init; }
    public Currency? Currency { get; internal set; }
    [Required] [MaxLength(128)] public string Description { get; private init; } = null!;
    [Required] public DateOnly ExecutionDate { get; private init; }
    [Required] public Status Status { get; private set; }
    public DateOnly? SubmissionDate { get; private set; }
    [Required] public Guid UserId { get; private init; }
    public User? User { get; internal set; }

    public static Expense Create(
        decimal amount,
        Category category,
        Currency currency,
        string description,
        DateOnly executionDate,
        User user) =>
        Create(Guid.NewGuid(), amount, category, currency, description, executionDate, user);

    private static Expense Create(
        Guid id,
        decimal amount,
        Category category,
        Currency currency,
        string description,
        DateOnly executionDate,
        User user) => new()
    {
        Id = id,
        Amount = amount,
        CategoryId = category.Id,
        CurrencyId = currency.Id,
        Description = description,
        ExecutionDate = executionDate,
        Status = Status.Draft,
        UserId = user.Id
    };

    public void Cancel()
    {
        Debug.Assert(Status is Status.Draft);
        Status = Status.Cancelled;
    }

    public void Submit()
    {
        Debug.Assert(Status is Status.Draft);
        Status = Status.Submitted;
        SubmissionDate = DateOnly.FromDateTime(DateTime.Now);
    }
}
