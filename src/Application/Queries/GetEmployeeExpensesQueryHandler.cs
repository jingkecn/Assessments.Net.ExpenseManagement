using Assessments.ExpenseManagement.Application.Extensions;
using Assessments.ExpenseManagement.Application.Models;
using Assessments.ExpenseManagement.Application.Specifications;
using Assessments.ExpenseManagement.Domain.Contracts;
using Assessments.ExpenseManagement.Domain.Exceptions;
using Assessments.ExpenseManagement.Domain.Models;
using FluentValidation;
using Mediator;

namespace Assessments.ExpenseManagement.Application.Queries;

public sealed class GetEmployeeExpensesQueryHandler(
    IReadRepository<Employee> employeeReadRepository,
    IReadRepository<Expense> expenseReadRepository,
    IValidator<GetEmployeeExpensesQuery> validator)
    : IQueryHandler<GetEmployeeExpensesQuery, (IEnumerable<ExpenseView> Expenses, int TotalCount)>
{
    public async ValueTask<(IEnumerable<ExpenseView> Expenses, int TotalCount)> Handle(
        GetEmployeeExpensesQuery query,
        CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(query, cancellationToken);

        var (employeeId, pageNumber, pageSize) = query;

        var getEmployeeByIdSpecification = new GetEmployeeByIdSpecification(employeeId);
        if (!await employeeReadRepository.ExistsAsync(getEmployeeByIdSpecification, cancellationToken))
        {
            throw DomainException.EmployeeNotFoundById(employeeId);
        }

        var getEmployeeExpensesSpecification = new GetEmployeeExpensesSpecification(employeeId);
        var totalCount = await expenseReadRepository.CountAsync(getEmployeeExpensesSpecification, cancellationToken);

        var getEmployeeExpensesWithAllSpecification = new GetEmployeeExpensesWithAllSpecification(employeeId);
        var expenses =
            await expenseReadRepository.GetAsync(getEmployeeExpensesWithAllSpecification, pageNumber,
                pageSize, cancellationToken);

        return (expenses.ToView(), totalCount);
    }
}
