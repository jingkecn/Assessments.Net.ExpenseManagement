using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Assessments.ExpenseManagement.Domain.Models;

[ComplexType]
public sealed record EmployeeInfo
{
    [Required] public string FirstName { get; init; } = null!;
    [Required] public string LastName { get; init; } = null!;
}
