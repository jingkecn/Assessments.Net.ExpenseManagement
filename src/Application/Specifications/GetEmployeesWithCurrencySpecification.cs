using Microsoft.EntityFrameworkCore;

namespace Assessments.ExpenseManagement.Application.Specifications;

public sealed class GetEmployeesWithCurrencySpecification : GetEmployeesSpecification
{
    public GetEmployeesWithCurrencySpecification() => Includes.Add(q => q.Include(e => e.Currency!));
}
