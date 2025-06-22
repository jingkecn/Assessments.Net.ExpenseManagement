using Assessments.ExpenseManagement.Application.Commands;
using Assessments.ExpenseManagement.Application.Models;
using Assessments.ExpenseManagement.Domain.Constants;
using FluentValidation;

namespace Assessments.ExpenseManagement.Application.Validations;

public sealed class AddEmployeeExpenseCommandValidator : AbstractValidator<AddEmployeeExpenseCommand>
{
    public AddEmployeeExpenseCommandValidator()
    {
        RuleFor(c => c.Amount)
            .GreaterThan(0);

        RuleFor(c => c.CategoryName)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(c => c.CurrencyCode)
            .NotEmpty()
            .MaximumLength(3)
            .MinimumLength(3);

        RuleFor(c => c.ExecutionDate)
            // .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now.AddMonths(-3)))
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now));

        RuleFor(c => c.Receipts)
            .NotEmpty()
            .ChildRules(validator => validator.RuleFor(e => e.All(ReceiptFileExists)))
            .WithMessage(ErrorMessages.ExpenseReceiptNotProvided);
    }

    private static bool ReceiptFileExists(ReceiptView receipt) =>
        // TODO: inject the CDN service and check the receipt file url.
        true;
}
