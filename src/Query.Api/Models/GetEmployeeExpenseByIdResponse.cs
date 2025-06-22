using System.ComponentModel.DataAnnotations;
using Assessments.ExpenseManagement.Application.Models;

namespace Assessments.ExpenseManagement.Query.Api.Models;

public sealed record GetEmployeeExpenseByIdResponse([property: Required] ExpenseView Expense);
