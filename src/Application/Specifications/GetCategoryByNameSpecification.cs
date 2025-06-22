using Assessments.ExpenseManagement.Application.Abstractions;
using Assessments.ExpenseManagement.Domain.Models;

namespace Assessments.ExpenseManagement.Application.Specifications;

public sealed class GetCategoryByNameSpecification(string name) : Specification<Category>(c => c.Name == name);
