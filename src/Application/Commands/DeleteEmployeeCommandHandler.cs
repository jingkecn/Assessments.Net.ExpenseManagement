using Assessments.ExpenseManagement.Application.Specifications;
using Assessments.ExpenseManagement.Domain.Contracts;
using Assessments.ExpenseManagement.Domain.Exceptions;
using Assessments.ExpenseManagement.Domain.Models;
using Mediator;

namespace Assessments.ExpenseManagement.Application.Commands;

public sealed class DeleteEmployeeCommandHandler(
    IReadRepository<Employee> employeeReadRepository,
    IWriteRepository<Employee> employeeWriteRepository) : ICommandHandler<DeleteEmployeeCommand, bool>
{
    public async ValueTask<bool> Handle(DeleteEmployeeCommand command, CancellationToken cancellationToken)
    {
        var specification = new GetEmployeeByIdSpecification(command.Id);
        if (!await employeeReadRepository.ExistsAsync(specification, cancellationToken))
        {
            throw DomainException.EmployeeNotFoundById(command.Id);
        }

        var employeeResults =
            await employeeReadRepository.GetAsync(specification, cancellationToken: cancellationToken);
        var employee = employeeResults.Single();
        var deleted = await employeeWriteRepository.DeleteAsync(employee, cancellationToken);
        _ = await employeeWriteRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
        return deleted;
    }
}
