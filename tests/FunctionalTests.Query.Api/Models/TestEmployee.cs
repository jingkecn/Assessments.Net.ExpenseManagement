using System.Diagnostics.CodeAnalysis;

namespace Assessments.ExpenseManagement.FunctionalTests.Query.Api.Models;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed record TestEmployee
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
}
