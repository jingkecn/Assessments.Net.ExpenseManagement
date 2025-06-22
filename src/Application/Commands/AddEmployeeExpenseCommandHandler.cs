using Assessments.ExpenseManagement.Application.Specifications;
using Assessments.ExpenseManagement.Domain.Contracts;
using Assessments.ExpenseManagement.Domain.Exceptions;
using Assessments.ExpenseManagement.Domain.Models;
using FluentValidation;
using Mediator;

namespace Assessments.ExpenseManagement.Application.Commands;

public sealed class AddEmployeeExpenseCommandHandler(
    IReadRepository<Category> categoryReadRepository,
    IReadRepository<Currency> currencyReadRepository,
    IReadRepository<Employee> employeeReadRepository,
    IWriteRepository<Expense> expenseWriteRepository,
    IWriteRepository<Receipt> receiptWriteRepository,
    IValidator<AddEmployeeExpenseCommand> validator) : ICommandHandler<AddEmployeeExpenseCommand, Guid>
{
    public async ValueTask<Guid> Handle(AddEmployeeExpenseCommand command, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(command, cancellationToken);

        var (amount, categoryName, currencyCode, employeeId, executionDate, receiptData) = command;

        var getCategoryByNameSpecification = new GetCategoryByNameSpecification(categoryName);
        if (!await categoryReadRepository.ExistsAsync(getCategoryByNameSpecification, cancellationToken))
        {
            throw DomainException.CategoryNotFoundByName(categoryName);
        }

        var categoryResults =
            await categoryReadRepository.GetAsync(getCategoryByNameSpecification, cancellationToken: cancellationToken);
        var category = categoryResults.Single();

        var getCurrencyByCodeSpecification = new GetCurrencyByCodeSpecification(currencyCode);
        if (!await currencyReadRepository.ExistsAsync(getCurrencyByCodeSpecification, cancellationToken))
        {
            throw DomainException.CurrencyNotFoundByCode(currencyCode);
        }

        var currencyResults =
            await currencyReadRepository.GetAsync(getCurrencyByCodeSpecification, cancellationToken: cancellationToken);
        var currency = currencyResults.Single();

        var getEmployeeByNameSpecification = new GetEmployeeByIdSpecification(employeeId);
        if (!await employeeReadRepository.ExistsAsync(getEmployeeByNameSpecification, cancellationToken))
        {
            throw DomainException.EmployeeNotFoundById(employeeId);
        }

        var employeeResults =
            await employeeReadRepository.GetAsync(getEmployeeByNameSpecification, cancellationToken: cancellationToken);
        var employee = employeeResults.Single();

        var receipts = receiptData.Select(rv => Receipt.Create(rv.Comment, rv.FileUrl)).ToList();
        foreach (var receipt in receipts)
        {
            _ = await receiptWriteRepository.AddAsync(receipt, cancellationToken);
        }

        var expense = Expense.Create(employee, amount, category, currency, executionDate, receipts);
        var result = await expenseWriteRepository.AddAsync(expense, cancellationToken);
        _ = await expenseWriteRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
        return result.Id;
    }
}
