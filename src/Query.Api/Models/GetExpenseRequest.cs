using System.Text.Json.Serialization;
using Assessments.ExpenseManagement.Application.Models;

namespace Assessments.ExpenseManagement.Query.Api.Models;

public sealed record GetExpenseRequest
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Guid? Id { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Guid? UserId { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public SortBy? SortBy { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public SortDirection? SortDirection { get; init; }

    public void Deconstruct(out Guid id, out Guid userId, out SortBy sortBy, out SortDirection sortDirection)
    {
        id = Id ?? Guid.Empty;
        userId = UserId ?? Guid.Empty;
        sortBy = SortBy ?? Application.Models.SortBy.Amount;
        sortDirection = SortDirection ?? Application.Models.SortDirection.Ascending;
    }
}
