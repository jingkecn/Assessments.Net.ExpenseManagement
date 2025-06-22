using Assessments.ExpenseManagement.Application.Extensions;
using Assessments.ExpenseManagement.Application.Models;
using Assessments.ExpenseManagement.Application.Specifications;
using Assessments.ExpenseManagement.Domain.Contracts;
using Assessments.ExpenseManagement.Domain.Models;
using FluentValidation;
using Mediator;

namespace Assessments.ExpenseManagement.Application.Queries;

public sealed class GetEmployeesQueryHandler(
    IReadRepository<Employee> employeeReadRepository,
    IValidator<GetEmployeesQuery> validator)
    : IQueryHandler<GetEmployeesQuery, (IEnumerable<EmployeeView> Employees, int TotalCount)>
{
    public async ValueTask<(IEnumerable<EmployeeView> Employees, int TotalCount)> Handle(GetEmployeesQuery query,
        CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(query, cancellationToken);

        var getEmployeesSpecification = new GetEmployeesSpecification();
        var totalCount = await employeeReadRepository.CountAsync(getEmployeesSpecification, cancellationToken);

        var getEmployeesWithCurrencySpecification = new GetEmployeesWithCurrencySpecification();
        var employees = await employeeReadRepository.GetAsync(
            getEmployeesWithCurrencySpecification,
            query.PageNumber,
            query.PageSize,
            cancellationToken);

        return (employees.ToView(), totalCount);
    }
}
