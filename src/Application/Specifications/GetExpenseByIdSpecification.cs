using Assessments.ExpenseManagement.Application.Abstractions;
using Assessments.ExpenseManagement.Domain.Models;

namespace Assessments.ExpenseManagement.Application.Specifications;

public class GetExpenseByIdSpecification(Guid id) : Specification<Expense>(e => e.Id == id);
