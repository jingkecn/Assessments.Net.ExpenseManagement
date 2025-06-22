using Assessments.ExpenseManagement.Application.Extensions;
using Assessments.ExpenseManagement.Application.Models;
using Assessments.ExpenseManagement.Application.Specifications;
using Assessments.ExpenseManagement.Domain.Contracts;
using Assessments.ExpenseManagement.Domain.Exceptions;
using Assessments.ExpenseManagement.Domain.Models;
using Mediator;

namespace Assessments.ExpenseManagement.Application.Queries;

public sealed class GetEmployeeExpenseByIdQueryHandler(
    IReadRepository<Employee> employeeReadRepository,
    IReadRepository<Expense> expenseReadRepository) : IQueryHandler<GetEmployeeExpenseByIdQuery, ExpenseView>
{
    public async ValueTask<ExpenseView> Handle(GetEmployeeExpenseByIdQuery query, CancellationToken cancellationToken)
    {
        var (employeeId, expenseId) = query;

        var getEmployeeByIdSpecification = new GetEmployeeByIdSpecification(employeeId);
        if (!await employeeReadRepository.ExistsAsync(getEmployeeByIdSpecification, cancellationToken))
        {
            throw DomainException.EmployeeNotFoundById(employeeId);
        }

        var getEmployeeExpenseByIdWithAllSpecification =
            new GetEmployeeExpenseByIdWithAllSpecification(employeeId, expenseId);
        if (!await expenseReadRepository.ExistsAsync(getEmployeeExpenseByIdWithAllSpecification, cancellationToken))
        {
            throw DomainException.ExpenseNotFoundByIdForEmployee(query.ExpenseId, query.ExpenseId);
        }

        var results = await expenseReadRepository
            .GetAsync(getEmployeeExpenseByIdWithAllSpecification, cancellationToken: cancellationToken);
        return results.Single().ToView();
    }
}
