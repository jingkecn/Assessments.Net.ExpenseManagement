using Assessments.ExpenseManagement.Domain.Abstractions;

namespace Assessments.ExpenseManagement.Domain.Contracts;

/// <summary>
///     The root <see cref="Entity" /> of an aggregate.
/// </summary>
/// <remarks>
///     <see href="https://deviq.com/domain-driven-design/aggregate-pattern">Aggregate Pattern</see>.
///     The aggregate root is responsible for controlling access to all the members of its aggregate,
///     and ensuring the consistency of the aggregate.
///     When considering how to structure your entities into aggregates,
///     a useful rule of thumb is to consider whether deletes should cascade.
/// </remarks>
public interface IAggregateRoot;
