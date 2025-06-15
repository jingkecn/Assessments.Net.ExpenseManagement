using Assessments.ExpenseManagement.Application.Specifications;
using Assessments.ExpenseManagement.Application.UseCases.Create;
using Assessments.ExpenseManagement.Domain.Contracts;
using Assessments.ExpenseManagement.Domain.Models;
using FluentValidation;

namespace Assessments.ExpenseManagement.Application.Validations;

public sealed class CreateExpenseCommandValidator : AbstractValidator<CreateExpenseCommand>
{
    public CreateExpenseCommandValidator(
        IReadRepository<Expense> expenseReadRepository,
        IReadRepository<User> userReadRepository)
    {
        RuleFor(c => c.Description).NotEmpty();

        RuleFor(c => c.ExecutionDate)
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now))
            .WithMessage("An expense cannot have a future date.");

        RuleFor(c => c.ExecutionDate)
            .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now.AddMonths(-3)))
            .WithMessage("An expense cannot be dated more than 3 months ago.");

        RuleFor(c => new { c.Amount, c.ExecutionDate, c.UserId })
            .MustAsync(async (t, cancellationToken) =>
            {
                var specification =
                    new GetExpenseByUserIdAndAmountAndExecutionDate(t.UserId, t.Amount, t.ExecutionDate);
                return !await expenseReadRepository.ExistsAsync(specification, cancellationToken);
            })
            .WithMessage("A user cannot declare the same expense twice (same date and amount).");

        RuleFor(c => new { c.CurrencyCode, c.UserId })
            .MustAsync(async (t, cancellationToken) =>
            {
                var specification = new GetUserByIdWithCurrencySpecification(t.UserId);
                var results = await userReadRepository.GetAsync(specification, cancellationToken);
                var user = results.Single();
                return user.Currency!.Code == t.CurrencyCode;
            })
            .WithMessage("The expense currency must be identical to the user's currency.");
    }
}
