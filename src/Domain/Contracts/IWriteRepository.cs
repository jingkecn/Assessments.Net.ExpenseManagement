using Assessments.ExpenseManagement.Domain.Abstractions;

namespace Assessments.ExpenseManagement.Domain.Contracts;

public interface IWriteRepository<TEntity> where TEntity : Entity
{
    IUnitOfWork UnitOfWork { get; }
    Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
}
