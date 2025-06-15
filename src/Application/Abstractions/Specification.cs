using System.Linq.Expressions;
using Assessments.ExpenseManagement.Domain.Abstractions;
using Assessments.ExpenseManagement.Domain.Contracts;
using Microsoft.EntityFrameworkCore.Query;

namespace Assessments.ExpenseManagement.Application.Abstractions;

/// <inheritdoc cref="ISpecification{TEntity}" />
public abstract class Specification<TEntity>(Expression<Func<TEntity, bool>> criteria)
    : ISpecification<TEntity> where TEntity : Entity
{
    public Expression<Func<TEntity, bool>> Criteria { get; } = criteria;
    public List<Expression<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>>> Includes { get; } = [];
    public virtual Expression<Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>>? OrderBy { get; protected init; }
}
