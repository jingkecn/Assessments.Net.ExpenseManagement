using System.ComponentModel.DataAnnotations;
using Assessments.ExpenseManagement.Domain.Models;
using Mediator;

namespace Assessments.ExpenseManagement.Application.Queries;

public record GetCategoryByNameQuery([property: Required] string Name) : IQuery<Category>;
