using Assessments.ExpenseManagement.Application.Specifications;
using Assessments.ExpenseManagement.Domain.Contracts;
using Assessments.ExpenseManagement.Domain.Exceptions;
using Assessments.ExpenseManagement.Domain.Models;
using FluentValidation;
using Mediator;

namespace Assessments.ExpenseManagement.Application.Commands;

public sealed class AddEmployeeCommandHandler(
    IReadRepository<Currency> currencyReadRepository,
    IWriteRepository<Employee> employeeWriteRepository,
    IValidator<AddEmployeeCommand> validator) : ICommandHandler<AddEmployeeCommand, Guid>
{
    public async ValueTask<Guid> Handle(AddEmployeeCommand command, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(command, cancellationToken);

        var (firstName, lastName, currencyCode) = command;

        var getCurrencyByCodeSpecification = new GetCurrencyByCodeSpecification(currencyCode);
        if (!await currencyReadRepository.ExistsAsync(getCurrencyByCodeSpecification, cancellationToken))
        {
            throw DomainException.CurrencyNotFoundByCode(currencyCode);
        }

        var currencyResults = await currencyReadRepository.GetAsync(
            getCurrencyByCodeSpecification, cancellationToken: cancellationToken);
        var currency = currencyResults.Single();

        var employeeId = await FetchEmployeeAsync(firstName, lastName);
        var employee = Employee.Create(employeeId, firstName, lastName, currency);
        await employeeWriteRepository.AddAsync(employee, cancellationToken);
        await employeeWriteRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
        return employee.Id;
    }

    private static Task<Guid> FetchEmployeeAsync(string firstName, string lastName) =>
        // TODO: fetch the employee id from the upstream employee management service.
        Task.FromResult(Guid.NewGuid());
}
