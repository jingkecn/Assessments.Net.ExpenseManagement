using System.ComponentModel.DataAnnotations;
using Assessments.ExpenseManagement.Domain.Abstractions;
using Assessments.ExpenseManagement.Domain.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Assessments.ExpenseManagement.Domain.Models;

[Index(nameof(Name), IsUnique = true)]
public sealed class Category : Entity, IAggregateRoot
{
    private Category() { }

    [Required] [MaxLength(50)] public string Name { get; private init; } = null!;

    public static Category Create(string name, bool isTransient = false) =>
        Create(isTransient ? Guid.Empty : Guid.NewGuid(), name);

    public static Category Create(Guid id, string name) => new() { Id = id, Name = name };
}
