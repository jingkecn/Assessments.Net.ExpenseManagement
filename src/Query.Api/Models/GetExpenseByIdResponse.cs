using System.ComponentModel.DataAnnotations;
using Assessments.ExpenseManagement.Application.Models;

namespace Assessments.ExpenseManagement.Query.Api.Models;

public sealed record GetExpenseByIdResponse([property: Required] ExpenseView Expense);
