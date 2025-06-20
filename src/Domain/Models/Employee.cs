using System.ComponentModel.DataAnnotations;
using Assessments.ExpenseManagement.Domain.Abstractions;
using Assessments.ExpenseManagement.Domain.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Assessments.ExpenseManagement.Domain.Models;

[Index(nameof(FirstName), nameof(LastName))]
public sealed class Employee : Entity, IAggregateRoot
{
    private Employee() { }

    [Required] [MaxLength(50)] public string FirstName { get; private init; } = null!;
    [Required] [MaxLength(50)] public string LastName { get; private init; } = null!;
    [Required] public Guid CurrencyId { get; private set; }
    public Currency? Currency { get; private set; }

    public static Employee Create(string firstName, string lastName, Currency currency, bool isTransient = false) =>
        Create(isTransient ? Guid.Empty : Guid.NewGuid(), firstName, lastName, currency);

    public static Employee Create(Guid id, string firstName, string lastName, Currency currency) =>
        new() { Id = id, FirstName = firstName, LastName = lastName, CurrencyId = currency.Id };

    public void UpdateCurrency(Currency currency) => CurrencyId = currency.Id;
}
