using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Assessments.ExpenseManagement.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Assessments.ExpenseManagement.Domain.Models;

[Index(nameof(FileUrl), IsUnique = true)]
public sealed class Receipt : Entity
{
    private Receipt() { }

    [Required] [MaxLength(500)] public string Comment { get; private set; }
    [Required] [MaxLength(500)] public string FileUrl { get; private init; }
    [NotMapped] public bool Exists => !string.IsNullOrEmpty(FileUrl); // TODO: check if the file exists in the storage.

    public static Receipt Create(string comment, string fileUrl, bool isTransient = false) =>
        new() { Id = isTransient ? Guid.Empty : Guid.NewGuid(), Comment = comment, FileUrl = fileUrl };
}
