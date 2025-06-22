using Assessments.ExpenseManagement.Application.Specifications;
using Assessments.ExpenseManagement.Domain.Contracts;
using Assessments.ExpenseManagement.Domain.Exceptions;
using Assessments.ExpenseManagement.Domain.Models;
using Mediator;

namespace Assessments.ExpenseManagement.Application.Commands;

public sealed class SubmitEmployeeExpenseCommandHandler(
    IReadRepository<Expense> expenseReadRepository,
    IWriteRepository<Expense> expenseWriteRepository) : ICommandHandler<SubmitEmployeeExpenseCommand, bool>
{
    public async ValueTask<bool> Handle(SubmitEmployeeExpenseCommand command, CancellationToken cancellationToken)
    {
        var (employeeId, expenseId) = command;

        var specification = new GetEmployeeExpenseByIdWithAllSpecification(employeeId, expenseId);
        if (!await expenseReadRepository.ExistsAsync(specification, cancellationToken))
        {
            throw DomainException.ExpenseNotFoundByIdForEmployee(expenseId, employeeId);
        }

        var expenseResults = await expenseReadRepository.GetAsync(specification, cancellationToken: cancellationToken);
        var expense = expenseResults.Single();
        expense.Submit();
        var modified = await expenseWriteRepository.UpdateAsync(expense, cancellationToken);
        _ = await expenseWriteRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
        return modified && expense.Status is Status.Submitted;
    }
}
