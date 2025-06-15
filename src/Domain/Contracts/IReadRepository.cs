using Assessments.ExpenseManagement.Domain.Abstractions;

namespace Assessments.ExpenseManagement.Domain.Contracts;

public interface IReadRepository<TEntity> where TEntity : Entity
{
    Task<int> CountAsync(
        ISpecification<TEntity> specification,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(
        ISpecification<TEntity> specification,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<TEntity>> GetAsync(
        ISpecification<TEntity> specification,
        CancellationToken cancellationToken = default);
}
