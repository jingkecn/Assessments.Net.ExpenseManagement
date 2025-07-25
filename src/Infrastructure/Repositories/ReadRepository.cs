using Assessments.ExpenseManagement.Domain.Abstractions;
using Assessments.ExpenseManagement.Domain.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Assessments.ExpenseManagement.Infrastructure.Repositories;

public class ReadRepository<TEntity>(ExpenseDbContext context)
    : IReadRepository<TEntity> where TEntity : Entity
{
    public async Task<int> CountAsync(
        ISpecification<TEntity> specification,
        CancellationToken cancellationToken = default)
    {
        var queryable = context.Set<TEntity>().AsQueryable();
        queryable = specification.Includes
            .Aggregate(queryable, (current, include) => include.Compile().Invoke(current));
        return await queryable.CountAsync(specification.Criteria, cancellationToken);
    }

    public async Task<bool> ExistsAsync(
        ISpecification<TEntity> specification,
        CancellationToken cancellationToken = default)
    {
        var queryable = context.Set<TEntity>().AsQueryable();
        queryable = specification.Includes
            .Aggregate(queryable, (current, include) => include.Compile().Invoke(current));
        return await queryable.AnyAsync(specification.Criteria, cancellationToken);
    }

    public async Task<IEnumerable<TEntity>> GetAsync(
        ISpecification<TEntity> specification,
        int pageNumber = 0,
        int pageSize = 0,
        CancellationToken cancellationToken = default)
    {
        var queryable = context.Set<TEntity>()
            .Where(specification.Criteria)
            .AsQueryable();
        if (pageNumber > 0 && pageSize > 0)
        {
            queryable = queryable
                .OrderBy(e => e.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);
        }

        queryable = specification.Includes
            .Aggregate(queryable, (current, include) => include.Compile().Invoke(current));
        return await queryable.ToListAsync(cancellationToken);
    }
}
