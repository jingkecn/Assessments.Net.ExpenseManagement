using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Assessments.ExpenseManagement.Domain.Models;

namespace Assessments.ExpenseManagement.Application.Models;

[SuppressMessage("ReSharper", "NotAccessedPositionalProperty.Global")]
public sealed record ExpenseView(
    [property: Required] Guid Id,
    [property: Required]
    [property: JsonIgnore]
    decimal Amount,
    [property: JsonIgnore] Category? Category,
    [property: JsonIgnore] Currency? Currency,
    [property: Required] string Description,
    [property: Required] DateOnly ExecutionDate,
    [property: Required]
    [property: JsonConverter(typeof(JsonStringEnumConverter))]
    Status Status,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    DateOnly? SubmissionDate,
    [property: JsonIgnore] User? User)
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? CategoryName => Category?.Name;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? AmountWithCurrency => Currency is null ? null : $"{Currency.Symbol}{Amount}";

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? UserName => User is null ? null : $"{User.FirstName} {User.LastName}";
}
