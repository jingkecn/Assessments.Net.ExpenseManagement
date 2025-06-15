using Assessments.ExpenseManagement.Application.Abstractions;
using Assessments.ExpenseManagement.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Assessments.ExpenseManagement.Application.Specifications;

public class GetExpensesByUserIdSpecification : Specification<Expense>
{
    protected GetExpensesByUserIdSpecification(Guid userId) : base(u => u.UserId == userId)
    {
        Includes.Add(q => q.Include(e => e.Category!));
        Includes.Add(q => q.Include(e => e.Currency!));
        Includes.Add(q => q.Include(e => e.User!));
    }
}
