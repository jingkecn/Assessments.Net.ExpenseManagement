using Assessments.ExpenseManagement.Application.Extensions;
using Assessments.ExpenseManagement.Application.Models;
using Assessments.ExpenseManagement.Application.Specifications;
using Assessments.ExpenseManagement.Domain.Contracts;
using Assessments.ExpenseManagement.Domain.Models;
using Mediator;

namespace Assessments.ExpenseManagement.Application.UseCases.Get;

public sealed class GetExpensesByUserIdQueryHandler(
    IReadRepository<Expense> expenseReadRepository) : IQueryHandler<GetExpensesByUserIdQuery, IEnumerable<ExpenseView>>
{
    public async ValueTask<IEnumerable<ExpenseView>> Handle(
        GetExpensesByUserIdQuery query,
        CancellationToken cancellationToken)
    {
        var (userId, sortBy, sortDirection) = query;
        ISpecification<Expense> specification = sortBy switch
        {
            SortBy.Amount when sortDirection is SortDirection.Ascending =>
                new GetExpensesByUserIdOrderByAmountAscendingSpecification(userId),
            SortBy.Amount when sortDirection is SortDirection.Descending =>
                new GetExpensesByUserIdOrderByAmountDescendingSpecification(userId),
            SortBy.ExecutionDate when sortDirection is SortDirection.Ascending =>
                new GetExpensesByUserIdOrderByExecutionDateAscendingSpecification(userId),
            SortBy.ExecutionDate when sortDirection is SortDirection.Descending =>
                new GetExpensesByUserIdOrderByExecutionDateDescendingSpecification(userId),
            _ => throw new ArgumentOutOfRangeException(nameof(query))
        };

        var expenses = await expenseReadRepository.GetAsync(specification, cancellationToken);
        return expenses.ToView();
    }
}
