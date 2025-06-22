using Assessments.ExpenseManagement.Application.Queries;
using FluentValidation;

namespace Assessments.ExpenseManagement.Application.Validations;

public sealed class GetExpensesQueryValidator : AbstractValidator<GetExpensesQuery>
{
    public GetExpensesQueryValidator()
    {
        RuleFor(c => c.PageNumber).GreaterThan(0);
        RuleFor(c => c.PageSize).GreaterThan(0);
    }
}
