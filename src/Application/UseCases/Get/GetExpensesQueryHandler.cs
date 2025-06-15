using Assessments.ExpenseManagement.Application.Extensions;
using Assessments.ExpenseManagement.Application.Models;
using Assessments.ExpenseManagement.Application.Specifications;
using Assessments.ExpenseManagement.Domain.Contracts;
using Assessments.ExpenseManagement.Domain.Models;
using Mediator;

namespace Assessments.ExpenseManagement.Application.UseCases.Get;

public sealed class GetExpensesQueryHandler(
    IReadRepository<Expense> expenseReadRepository) : IQueryHandler<GetExpensesQuery, IEnumerable<ExpenseView>>
{
    public async ValueTask<IEnumerable<ExpenseView>> Handle(
        GetExpensesQuery query,
        CancellationToken cancellationToken)
    {
        var (sortBy, sortDirection) = query;
        ISpecification<Expense> specification = sortBy switch
        {
            SortBy.Amount when sortDirection is SortDirection.Ascending =>
                new GetExpensesOrderByAmountAscendingSpecification(),
            SortBy.Amount when sortDirection is SortDirection.Descending =>
                new GetExpensesOrderByAmountDescendingSpecification(),
            SortBy.ExecutionDate when sortDirection is SortDirection.Ascending =>
                new GetExpensesOrderByExecutionDateAscendingSpecification(),
            SortBy.ExecutionDate when sortDirection is SortDirection.Descending =>
                new GetExpensesOrderByExecutionDateDescendingSpecification(),
            _ => throw new ArgumentOutOfRangeException(nameof(query))
        };

        var expenses = await expenseReadRepository.GetAsync(specification, cancellationToken);
        return expenses.ToView();
    }
}
