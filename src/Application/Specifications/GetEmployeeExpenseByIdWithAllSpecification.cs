using Microsoft.EntityFrameworkCore;

namespace Assessments.ExpenseManagement.Application.Specifications;

public sealed class GetEmployeeExpenseByIdWithAllSpecification : GetEmployeeExpenseByIdSpecification
{
    public GetEmployeeExpenseByIdWithAllSpecification(Guid employeeId, Guid expenseId) : base(employeeId, expenseId)
    {
        Includes.Add(q => q.Include(e => e.Category!));
        Includes.Add(q => q.Include(e => e.Currency!));
        Includes.Add(q => q.Include(e => e.Employee!).ThenInclude(e => e.Currency!));
        Includes.Add(q => q.Include(e => e.Receipts));
    }
}
