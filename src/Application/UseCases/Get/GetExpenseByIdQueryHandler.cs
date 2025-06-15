using Assessments.ExpenseManagement.Application.Extensions;
using Assessments.ExpenseManagement.Application.Models;
using Assessments.ExpenseManagement.Application.Specifications;
using Assessments.ExpenseManagement.Domain.Contracts;
using Assessments.ExpenseManagement.Domain.Exceptions;
using Assessments.ExpenseManagement.Domain.Models;
using Mediator;

namespace Assessments.ExpenseManagement.Application.UseCases.Get;

public sealed class GetExpenseByIdQueryHandler(
    IReadRepository<Expense> expenseReadRepository) : IQueryHandler<GetExpenseByIdQuery, ExpenseView>
{
    public async ValueTask<ExpenseView> Handle(GetExpenseByIdQuery query, CancellationToken cancellationToken)
    {
        var id = query.Id;
        var specification = new GetExpenseByIdSpecification(id);
        if (!await expenseReadRepository.ExistsAsync(specification, cancellationToken))
        {
            throw DomainException.ExpenseNotFoundById(id);
        }

        var expenses = await expenseReadRepository.GetAsync(specification, cancellationToken);
        return expenses.Single().ToView();
    }
}
