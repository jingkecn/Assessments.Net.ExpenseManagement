using Assessments.ExpenseManagement.Application.Abstractions;
using Assessments.ExpenseManagement.Domain.Models;

namespace Assessments.ExpenseManagement.Application.Specifications;

public class GetEmployeeByIdSpecification(Guid id) : Specification<Employee>(e => e.Id == id);
