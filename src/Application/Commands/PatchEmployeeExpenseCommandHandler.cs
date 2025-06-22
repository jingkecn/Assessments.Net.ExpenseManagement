using Assessments.ExpenseManagement.Domain.Models;
using FluentValidation;
using Mediator;

namespace Assessments.ExpenseManagement.Application.Commands;

public sealed class PatchEmployeeExpenseCommandHandler(
    IMediator mediator,
    IValidator<PatchEmployeeExpenseCommand> validator) : ICommandHandler<PatchEmployeeExpenseCommand, bool>
{
    public async ValueTask<bool> Handle(PatchEmployeeExpenseCommand command, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(command, cancellationToken);
        return command.Action switch
        {
            var action when action.Equals(nameof(Expense.Cancel), StringComparison.OrdinalIgnoreCase) =>
                await CancelAsync(command, cancellationToken),
            var action when action.Equals(nameof(Expense.Submit), StringComparison.OrdinalIgnoreCase) =>
                await SubmitAsync(command, cancellationToken),
            _ => false
        };
    }

    private async ValueTask<bool> CancelAsync(PatchEmployeeExpenseCommand request, CancellationToken cancellationToken)
    {
        var command = new CancelEmployeeExpenseCommand(request.EmployeeId, request.ExpenseId);
        return await mediator.Send(command, cancellationToken);
    }

    private async ValueTask<bool> SubmitAsync(PatchEmployeeExpenseCommand request, CancellationToken cancellationToken)
    {
        var command = new SubmitEmployeeExpenseCommand(request.EmployeeId, request.ExpenseId);
        return await mediator.Send(command, cancellationToken);
    }
}
