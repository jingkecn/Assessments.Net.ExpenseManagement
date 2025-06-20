using System.ComponentModel.DataAnnotations;
using Assessments.ExpenseManagement.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Assessments.ExpenseManagement.Domain.Models;

[Index(nameof(Name), IsUnique = true)]
public sealed class Category : Entity
{
    private Category() { }

    [Required] [MaxLength(50)] public string Name { get; private init; }

    public static Category Create(string name, bool isTransient = false) =>
        new() { Id = isTransient ? Guid.Empty : Guid.NewGuid(), Name = name };

    /// <summary>
    ///     Checks if the category is allowed for the given employee.
    ///     This could involve checking against a list of allowed categories for that employee.
    /// </summary>
    /// <param name="employee">The employee.</param>
    /// <returns><c>true</c> if this category is allowed for the employee, otherwise <c>false</c>.</returns>
    public bool IsAllowedFor(Employee employee) => employee.Id != Guid.Empty;
}
