using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Assessments.ExpenseManagement.Domain.Models;

/// <summary>
///     Represents a amount value.
/// </summary>
/// <remarks>
///     [!NOTE]
///     We’re not using a primary constructor for the Contact type because EF Core does not yet support constructor
///     injection of complex type values.
///     Vote for <see href="https://github.com/dotnet/efcore/issues/31621">Issue #31621</see> if this is important to you.
/// </remarks>
[ComplexType]
public sealed record Amount
{
    [NotMapped] public bool IsValid => Value > 0;

    [Required] public decimal Value { get; init; }
}
