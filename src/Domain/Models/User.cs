using System.ComponentModel.DataAnnotations;
using Assessments.ExpenseManagement.Domain.Abstractions;
using Assessments.ExpenseManagement.Domain.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Assessments.ExpenseManagement.Domain.Models;

[Index(nameof(FirstName), nameof(LastName))]
public sealed class User : Entity, IAggregateRoot
{
    private User() { }

    [Required] [MaxLength(50)] public string FirstName { get; private set; } = null!;
    [Required] [MaxLength(50)] public string LastName { get; private set; } = null!;
    [Required] public Guid CurrencyId { get; private set; }
    public Currency? Currency { get; internal set; }

    public static User Create(Guid id, string firstName, string lastName, Currency currency) =>
        new() { Id = id, FirstName = firstName, LastName = lastName, CurrencyId = currency.Id };
}
