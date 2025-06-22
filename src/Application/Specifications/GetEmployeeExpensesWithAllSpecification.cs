using Microsoft.EntityFrameworkCore;

namespace Assessments.ExpenseManagement.Application.Specifications;

public sealed class GetEmployeeExpensesWithAllSpecification : GetEmployeeExpensesSpecification
{
    public GetEmployeeExpensesWithAllSpecification(Guid employeeId) : base(employeeId)
    {
        Includes.Add(q => q.Include(e => e.Category!));
        Includes.Add(q => q.Include(e => e.Currency!));
        Includes.Add(q => q.Include(e => e.Employee!).ThenInclude(e => e.Currency!));
        Includes.Add(q => q.Include(e => e.Receipts));
    }
}
