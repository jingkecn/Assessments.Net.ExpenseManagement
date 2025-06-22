using System.Diagnostics;
using Assessments.ExpenseManagement.Domain.Abstractions;
using Assessments.ExpenseManagement.Domain.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Assessments.ExpenseManagement.Infrastructure.Repositories;

public class WriteRepository<TEntity>(ExpenseDbContext context)
    : IWriteRepository<TEntity> where TEntity : Entity
{
    public IUnitOfWork UnitOfWork { get; } = context;

    public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        Debug.Assert(!entity.IsTransient);
        var entry = await context.Set<TEntity>().AddAsync(entity, cancellationToken);
        return entry.Entity;
    }

    public Task<bool> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        Debug.Assert(!entity.IsTransient);
        var entry = context.Set<TEntity>().Remove(entity);
        return Task.FromResult(entry.State is EntityState.Deleted);
    }

    public Task<bool> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        Debug.Assert(!entity.IsTransient);
        var entry = context.Set<TEntity>().Update(entity);
        return Task.FromResult(entry.State is EntityState.Modified);
    }
}
