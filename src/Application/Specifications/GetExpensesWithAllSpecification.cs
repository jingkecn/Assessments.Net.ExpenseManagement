using Microsoft.EntityFrameworkCore;

namespace Assessments.ExpenseManagement.Application.Specifications;

public sealed class GetExpensesWithAllSpecification : GetExpensesSpecification
{
    public GetExpensesWithAllSpecification()
    {
        Includes.Add(q => q.Include(e => e.Category!));
        Includes.Add(q => q.Include(e => e.Currency!));
        Includes.Add(q => q.Include(e => e.Employee!));
        Includes.Add(q => q.Include(e => e.Receipts));
    }
}
