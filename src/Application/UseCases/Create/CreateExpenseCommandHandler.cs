using Assessments.ExpenseManagement.Application.Specifications;
using Assessments.ExpenseManagement.Domain.Contracts;
using Assessments.ExpenseManagement.Domain.Exceptions;
using Assessments.ExpenseManagement.Domain.Models;
using FluentValidation;
using Mediator;

namespace Assessments.ExpenseManagement.Application.UseCases.Create;

public sealed class CreateExpenseCommandHandler(
    IReadRepository<Category> categoryReadRepository,
    IReadRepository<Currency> currencyReadRepository,
    IWriteRepository<Expense> expenseWriteRepository,
    IReadRepository<User> userReadRepository,
    IValidator<CreateExpenseCommand> validator) : ICommandHandler<CreateExpenseCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateExpenseCommand command, CancellationToken cancellationToken)
    {
        var (amount, categoryName, currencyCode, description, executionDate, userId) = command;

        var getCategoryByNameSpecification = new GetCategoryByNameSpecification(categoryName);
        if (!await categoryReadRepository.ExistsAsync(getCategoryByNameSpecification, cancellationToken))
        {
            throw DomainException.CategoryNotFoundByName(categoryName);
        }

        var categoryResults = await categoryReadRepository.GetAsync(getCategoryByNameSpecification, cancellationToken);
        var category = categoryResults.Single();

        var getCurrencyByCodeSpecification = new GetCurrencyByCodeSpecification(currencyCode);
        if (!await currencyReadRepository.ExistsAsync(getCurrencyByCodeSpecification, cancellationToken))
        {
            throw DomainException.CurrencyNotFoundByCode(currencyCode);
        }

        var currencyResults =
            await currencyReadRepository.GetAsync(getCurrencyByCodeSpecification, cancellationToken);
        var currency = currencyResults.Single();

        var getUserByIdSpecification = new GetUserByIdSpecification(userId);
        if (!await userReadRepository.ExistsAsync(getUserByIdSpecification, cancellationToken))
        {
            throw DomainException.UserNotFoundById(userId);
        }

        var userResults = await userReadRepository.GetAsync(getUserByIdSpecification, cancellationToken);
        var user = userResults.Single();

        await validator.ValidateAndThrowAsync(command, cancellationToken);

        var expense = Expense.Create(amount, category, currency, description, executionDate, user);
        var result = await expenseWriteRepository.AddAsync(expense, cancellationToken);
        _ = await expenseWriteRepository.SaveChangesAsync(cancellationToken);
        return result.Id;
    }
}
