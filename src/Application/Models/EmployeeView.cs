using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Assessments.ExpenseManagement.Application.Models;

public sealed record EmployeeView(
    [property: Required] string FirstName,
    [property: Required] string LastName,
    [property: Required]
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [property: MaxLength(3)]
    [property: MinLength(3)]
    string? Currency = null);
