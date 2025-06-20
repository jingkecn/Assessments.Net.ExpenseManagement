using System.ComponentModel.DataAnnotations;
using Assessments.ExpenseManagement.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Assessments.ExpenseManagement.Domain.Models;

[Index(nameof(FileUrl), IsUnique = true)]
public sealed class Receipt : Entity
{
    private Receipt() { }

    [Required] [MaxLength(500)] public string Comment { get; private set; } = null!;
    [Required] [MaxLength(500)] public string FileUrl { get; private init; } = null!;

    public static Receipt Create(string comment, string fileUrl, bool isTransient = false) =>
        Create(isTransient ? Guid.Empty : Guid.NewGuid(), comment, fileUrl);

    public static Receipt Create(Guid id, string comment, string fileUrl) =>
        new() { Id = id, Comment = comment, FileUrl = fileUrl };
}
