using Assessments.ExpenseManagement.Application.Abstractions;
using Assessments.ExpenseManagement.Domain.Models;

namespace Assessments.ExpenseManagement.Application.Specifications;

public class GetUserByIdSpecification(Guid id) : Specification<User>(u => u.Id == id);
