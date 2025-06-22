using Assessments.ExpenseManagement.Application.Abstractions;
using Assessments.ExpenseManagement.Domain.Models;

namespace Assessments.ExpenseManagement.Application.Specifications;

public class GetEmployeeExpensesSpecification(Guid employeeId)
    : Specification<Expense>(e => e.EmployeeId == employeeId);
