using Microsoft.EntityFrameworkCore;

namespace Assessments.ExpenseManagement.Application.Specifications;

public sealed class GetUserByIdWithCurrencySpecification : GetUserByIdSpecification
{
    public GetUserByIdWithCurrencySpecification(Guid id) : base(id) =>
        Includes.Add(q => q.Include(u => u.Currency!));
}
