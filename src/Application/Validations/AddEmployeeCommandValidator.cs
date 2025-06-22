using Assessments.ExpenseManagement.Application.Commands;
using FluentValidation;

namespace Assessments.ExpenseManagement.Application.Validations;

public sealed class AddEmployeeCommandValidator : AbstractValidator<AddEmployeeCommand>
{
    public AddEmployeeCommandValidator()
    {
        RuleFor(c => c.CurrencyCode)
            .NotEmpty()
            .MaximumLength(3)
            .MinimumLength(3);

        RuleFor(c => c.FirstName)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(c => c.LastName)
            .NotEmpty()
            .MaximumLength(50);
    }
}
