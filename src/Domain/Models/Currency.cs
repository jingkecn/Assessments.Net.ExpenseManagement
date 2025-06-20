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

    [Required] [MaxLength(50)] public string Name { get; private init; } = null!;
    [Required] [MaxLength(1)] public string Symbol { get; private init; } = null!;

    public static Currency Create(string code, string name, string symbol, bool isTransient = false) =>
        Create(isTransient ? Guid.Empty : Guid.NewGuid(), code, name, symbol);

    public static Currency Create(Guid id, string code, string name, string symbol) =>
        new() { Id = id, Code = code, Name = name, Symbol = symbol };
}
