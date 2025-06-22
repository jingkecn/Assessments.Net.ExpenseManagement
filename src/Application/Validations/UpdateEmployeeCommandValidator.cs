using Assessments.ExpenseManagement.Application.Commands;
using FluentValidation;

namespace Assessments.ExpenseManagement.Application.Validations;

public sealed class UpdateEmployeeCommandValidator : AbstractValidator<UpdateEmployeeCommand>
{
    public UpdateEmployeeCommandValidator() =>
        RuleFor(c => c.CurrencyCode)
            .NotEmpty()
            .MaximumLength(3)
            .MinimumLength(3);
}
