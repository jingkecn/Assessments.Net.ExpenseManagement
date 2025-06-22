using Assessments.ExpenseManagement.Application.Extensions;
using Assessments.ExpenseManagement.Application.Models;
using Assessments.ExpenseManagement.Application.Specifications;
using Assessments.ExpenseManagement.Domain.Contracts;
using Assessments.ExpenseManagement.Domain.Models;
using FluentValidation;
using Mediator;

namespace Assessments.ExpenseManagement.Application.Queries;

public sealed class GetExpensesQueryHandler(
    IReadRepository<Expense> expenseReadRepository,
    IValidator<GetExpensesQuery> validator)
    : IQueryHandler<GetExpensesQuery, (IEnumerable<ExpenseView> Expenses, int TotalCount)>
{
    public async ValueTask<(IEnumerable<ExpenseView> Expenses, int TotalCount)> Handle(
        GetExpensesQuery query,
        CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(query, cancellationToken);

        var getExpensesSpecification = new GetExpensesSpecification();
        var totalCount = await expenseReadRepository.CountAsync(getExpensesSpecification, cancellationToken);

        var getExpensesWithAllSpecification = new GetExpensesWithAllSpecification();
        var expenses =
            await expenseReadRepository.GetAsync(getExpensesWithAllSpecification, query.PageNumber, query.PageSize,
                cancellationToken);

        return (expenses.ToView(), totalCount);
    }
}
