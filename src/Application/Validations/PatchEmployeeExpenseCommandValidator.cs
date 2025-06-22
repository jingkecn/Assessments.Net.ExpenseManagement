using System.Text;
using Assessments.ExpenseManagement.Application.Commands;
using Assessments.ExpenseManagement.Domain.Constants;
using Assessments.ExpenseManagement.Domain.Models;
using FluentValidation;

namespace Assessments.ExpenseManagement.Application.Validations;

public sealed class PatchEmployeeExpenseCommandValidator : AbstractValidator<PatchEmployeeExpenseCommand>
{
    private static readonly IEnumerable<string> SupportedExpenseActions =
        [nameof(Expense.Cancel), nameof(Expense.Submit)];

    public PatchEmployeeExpenseCommandValidator() =>
        RuleFor(c => c.Action)
            .NotEmpty()
            .Must(action => SupportedExpenseActions.Contains(action, StringComparer.OrdinalIgnoreCase))
            .WithMessage(string.Format(
                null,
                CompositeFormat.Parse(ErrorMessages.ExpenseInvalidAction),
                string.Join("|", SupportedExpenseActions)));
}
