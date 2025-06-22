using Assessments.ExpenseManagement.Application.Queries;
using FluentValidation;

namespace Assessments.ExpenseManagement.Application.Validations;

public sealed class GetEmployeeExpensesQueryValidator : AbstractValidator<GetEmployeeExpensesQuery>
{
    public GetEmployeeExpensesQueryValidator()
    {
        RuleFor(c => c.PageNumber).GreaterThan(0);
        RuleFor(c => c.PageSize).GreaterThan(0);
    }
}
