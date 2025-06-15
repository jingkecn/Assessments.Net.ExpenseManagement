using System.ComponentModel.DataAnnotations;
using Assessments.ExpenseManagement.Domain.Abstractions;
using Assessments.ExpenseManagement.Domain.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Assessments.ExpenseManagement.Domain.Models;

[Index(nameof(Code), IsUnique = true)]
public sealed class Currency : Entity, IAggregateRoot
{
    private Currency() { }

    [Required]
    [MaxLength(3)]
    [MinLength(3)]
    public string Code { get; private init; } = null!;

    [Required] public char Symbol { get; private init; }

    public static Currency Create(Guid id, string code, char symbol) => new() { Id = id, Code = code, Symbol = symbol };
}
