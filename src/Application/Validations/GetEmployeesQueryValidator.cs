using Assessments.ExpenseManagement.Application.Queries;
using FluentValidation;

namespace Assessments.ExpenseManagement.Application.Validations;

public sealed class GetEmployeesQueryValidator : AbstractValidator<GetEmployeesQuery>
{
    public GetEmployeesQueryValidator()
    {
        RuleFor(c => c.PageNumber).GreaterThan(0);
        RuleFor(c => c.PageSize).GreaterThan(0);
    }
}
