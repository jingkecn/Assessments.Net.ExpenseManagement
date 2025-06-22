using Assessments.ExpenseManagement.Application.Specifications;
using Assessments.ExpenseManagement.Domain.Contracts;
using Assessments.ExpenseManagement.Domain.Exceptions;
using Assessments.ExpenseManagement.Domain.Models;
using FluentValidation;
using Mediator;

namespace Assessments.ExpenseManagement.Application.Commands;

public sealed class UpdateEmployeeCommandHandler(
    IReadRepository<Currency> currencyReadRepository,
    IReadRepository<Employee> employeeReadRepository,
    IWriteRepository<Employee> employeeWriteRepository,
    IValidator<UpdateEmployeeCommand> validator) : ICommandHandler<UpdateEmployeeCommand, bool>
{
    public async ValueTask<bool> Handle(UpdateEmployeeCommand command, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(command, cancellationToken);

        var (employeeId, currencyCode) = command;

        var getCurrencyByCodeSpecification = new GetCurrencyByCodeSpecification(currencyCode);
        if (!await currencyReadRepository.ExistsAsync(getCurrencyByCodeSpecification, cancellationToken))
        {
            throw DomainException.CurrencyNotFoundByCode(currencyCode);
        }

        var currencies = await currencyReadRepository.GetAsync(
            getCurrencyByCodeSpecification, cancellationToken: cancellationToken);
        var currency = currencies.Single();

        var getEmployeeByIdSpecification = new GetEmployeeByIdSpecification(employeeId);
        if (!await employeeReadRepository.ExistsAsync(getEmployeeByIdSpecification, cancellationToken))
        {
            throw DomainException.EmployeeNotFoundById(employeeId);
        }

        var employees = await employeeReadRepository.GetAsync(
            getEmployeeByIdSpecification, cancellationToken: cancellationToken);
        var employee = employees.Single();
        employee.UpdateCurrency(currency);
        var modified = await employeeWriteRepository.UpdateAsync(employee, cancellationToken);
        _ = await employeeWriteRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
        return modified;
    }
}
