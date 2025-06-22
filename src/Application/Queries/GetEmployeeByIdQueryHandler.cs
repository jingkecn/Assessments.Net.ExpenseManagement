using Assessments.ExpenseManagement.Application.Extensions;
using Assessments.ExpenseManagement.Application.Models;
using Assessments.ExpenseManagement.Application.Specifications;
using Assessments.ExpenseManagement.Domain.Contracts;
using Assessments.ExpenseManagement.Domain.Exceptions;
using Assessments.ExpenseManagement.Domain.Models;
using Mediator;

namespace Assessments.ExpenseManagement.Application.Queries;

public sealed class GetEmployeeByIdQueryHandler(IReadRepository<Employee> employeeReadRepository)
    : IQueryHandler<GetEmployeeByIdQuery, EmployeeView>
{
    public async ValueTask<EmployeeView> Handle(GetEmployeeByIdQuery query, CancellationToken cancellationToken)
    {
        var specification = new GetEmployeeByIdWithCurrencySpecification(query.Id);
        if (!await employeeReadRepository.ExistsAsync(specification, cancellationToken))
        {
            throw DomainException.EmployeeNotFoundById(query.Id);
        }

        var results =
            await employeeReadRepository.GetAsync(specification, cancellationToken: cancellationToken);
        return results.Single().ToView();
    }
}
