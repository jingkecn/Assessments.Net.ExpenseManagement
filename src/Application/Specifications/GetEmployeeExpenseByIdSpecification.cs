using Assessments.ExpenseManagement.Application.Abstractions;
using Assessments.ExpenseManagement.Domain.Models;

namespace Assessments.ExpenseManagement.Application.Specifications;

public class GetEmployeeExpenseByIdSpecification(Guid employeeId, Guid expenseId)
    : Specification<Expense>(e => e.Id == expenseId && e.EmployeeId == employeeId)
{
}
