using Assessments.ExpenseManagement.Application.Extensions;
using Assessments.ExpenseManagement.Application.Models;
using Assessments.ExpenseManagement.Application.Specifications;
using Assessments.ExpenseManagement.Domain.Contracts;
using Assessments.ExpenseManagement.Domain.Exceptions;
using Assessments.ExpenseManagement.Domain.Models;
using Mediator;

namespace Assessments.ExpenseManagement.Application.Queries;

public sealed class GetExpenseByIdQueryHandler(IReadRepository<Expense> expenseReadRepository)
    : IQueryHandler<GetExpenseByIdQuery, ExpenseView>
{
    public async ValueTask<ExpenseView> Handle(GetExpenseByIdQuery query, CancellationToken cancellationToken)
    {
        var specification = new GetExpenseByIdWithAllExceptEmployeeSpecification(query.Id);
        if (!await expenseReadRepository.ExistsAsync(specification, cancellationToken))
        {
            throw DomainException.ExpenseNotFoundById(query.Id);
        }

        var expenses = await expenseReadRepository.GetAsync(specification, cancellationToken: cancellationToken);
        return expenses.Single().ToView();
    }
}
