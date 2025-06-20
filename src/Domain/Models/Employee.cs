using System.ComponentModel.DataAnnotations;
using Assessments.ExpenseManagement.Domain.Abstractions;
using Assessments.ExpenseManagement.Domain.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Assessments.ExpenseManagement.Domain.Models;

[Index(nameof(FirstName), nameof(LastName), IsUnique = true)]
public sealed class Employee : Entity, IAggregateRoot
{
    private Employee() { }

    [Required] [MaxLength(50)] public string FirstName { get; private init; }
    [Required] [MaxLength(50)] public string LastName { get; private init; }

    [Required]
    [MaxLength(3)]
    [MinLength(3)]
    public string CurrencyCode { get; private set; }

    public static Employee Create(string firstName, string lastName, string currencyCode, bool isTransient = false) =>
        new()
        {
            Id = isTransient ? Guid.Empty : Guid.NewGuid(),
            FirstName = firstName,
            LastName = lastName,
            CurrencyCode = currencyCode
        };

    public static Employee Create(Guid id, string firstName, string lastName, string currencyCode) =>
        new() { Id = id, FirstName = firstName, LastName = lastName, CurrencyCode = currencyCode };

    internal void UpdateCurrencyCode(string currencyCode) => CurrencyCode = currencyCode;
}
