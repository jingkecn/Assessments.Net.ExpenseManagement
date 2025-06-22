using Assessments.ExpenseManagement.Application.Specifications;
using Assessments.ExpenseManagement.Domain.Contracts;
using Assessments.ExpenseManagement.Domain.Exceptions;
using Assessments.ExpenseManagement.Domain.Models;
using Mediator;

namespace Assessments.ExpenseManagement.Application.Commands;

public sealed class DeleteExpenseCommandHandler(
    IReadRepository<Expense> expenseReadRepository,
    IWriteRepository<Expense> expenseWriteRepository) : ICommandHandler<DeleteExpenseCommand, bool>
{
    public async ValueTask<bool> Handle(DeleteExpenseCommand command, CancellationToken cancellationToken)
    {
        var specification = new GetExpenseByIdSpecification(command.Id);
        if (!await expenseReadRepository.ExistsAsync(specification, cancellationToken))
        {
            throw DomainException.ExpenseNotFoundById(command.Id);
        }

        var expenseResults = await expenseReadRepository.GetAsync(specification, cancellationToken: cancellationToken);
        var expense = expenseResults.Single();
        var deleted = await expenseWriteRepository.DeleteAsync(expense, cancellationToken);
        _ = await expenseWriteRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
        return deleted;
    }
}
