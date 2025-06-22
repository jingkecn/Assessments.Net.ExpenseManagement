using Microsoft.EntityFrameworkCore;

namespace Assessments.ExpenseManagement.Application.Specifications;

public sealed class GetEmployeeByIdWithCurrencySpecification : GetEmployeeByIdSpecification
{
    public GetEmployeeByIdWithCurrencySpecification(Guid id) : base(id) =>
        Includes.Add(q => q.Include(e => e.Currency!));
}
