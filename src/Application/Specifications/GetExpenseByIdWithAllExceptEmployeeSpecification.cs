using Microsoft.EntityFrameworkCore;

namespace Assessments.ExpenseManagement.Application.Specifications;

public sealed class GetExpenseByIdWithAllExceptEmployeeSpecification : GetExpenseByIdSpecification
{
    public GetExpenseByIdWithAllExceptEmployeeSpecification(Guid id) : base(id)
    {
        Includes.Add(q => q.Include(e => e.Category!));
        Includes.Add(q => q.Include(e => e.Currency!));
        Includes.Add(q => q.Include(e => e.Receipts));
    }
}
