using System.ComponentModel.DataAnnotations;

namespace Assessments.ExpenseManagement.Command.Api.Models;

public sealed record UpdateEmployeeRequest(
    [property: Required]
    [property: MaxLength(3)]
    [property: MinLength(3)]
    string CurrencyCode);
