using System.ComponentModel.DataAnnotations;
using Assessments.ExpenseManagement.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Assessments.ExpenseManagement.Domain.Models;

[Index(nameof(Code), IsUnique = true)]
public sealed class Currency : Entity
{
    private Currency() { }

    [Required]
    [MaxLength(3)]
    [MinLength(3)]
    public string Code { get; private init; }

    [Required] [MaxLength(50)] public string Name { get; private init; }
    [Required] [MaxLength(1)] public string Symbol { get; private init; }

    public static Currency Create(string code, string name, string symbol, bool isTransient = false) =>
        new() { Id = isTransient ? Guid.Empty : Guid.NewGuid(), Code = code, Name = name, Symbol = symbol };

    public static Currency Create(Guid id, string code, string name, string symbol) =>
        new() { Id = id, Code = code, Name = name, Symbol = symbol };

    public bool IsAllowedFor(Employee employee) => employee.CurrencyCode == Code;
}
