using System.Linq.Expressions;
using Assessments.ExpenseManagement.Domain.Abstractions;
using Microsoft.EntityFrameworkCore.Query;

namespace Assessments.ExpenseManagement.Domain.Contracts;

/// <summary>
///     Describes a query in an object.
/// </summary>
/// <remarks>
///     <see href="https://deviq.com/design-patterns/specification-pattern">Specification Pattern</see>.
///     The Specification design pattern eliminates the need for lazy loading in web applications (bad idea),
///     and helps keep repository implementations from becoming cluttered with these details.
/// </remarks>
/// <typeparam name="TEntity">The <see cref="Entity" /> type.</typeparam>
public interface ISpecification<TEntity> where TEntity : Entity
{
    Expression<Func<TEntity, bool>> Criteria { get; }
    List<Expression<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>>> Includes { get; }
}
