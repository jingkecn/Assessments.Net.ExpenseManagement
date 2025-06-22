using System.Diagnostics.CodeAnalysis;

namespace Assessments.ExpenseManagement.FunctionalTests.Query.Api.Models;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
public sealed class TestEntity
{
    public Guid Id { get; init; }
}
