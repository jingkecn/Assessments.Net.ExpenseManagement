using System.ComponentModel.DataAnnotations;
using Assessments.ExpenseManagement.Domain.Models;

namespace Assessments.ExpenseManagement.Query.Api.Models;

public sealed record GetCategoryByNameResponse([property: Required] Category Category);
