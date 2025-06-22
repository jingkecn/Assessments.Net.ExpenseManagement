using System.ComponentModel.DataAnnotations;

namespace Assessments.ExpenseManagement.Command.Api.Models;

public sealed record AddEmployeeRequest(
    [property: Required] string FirstName,
    [property: Required] string LastName,
    [property: Required]
    [property: MaxLength(3)]
    [property: MinLength(3)]
    string CurrencyCode);
