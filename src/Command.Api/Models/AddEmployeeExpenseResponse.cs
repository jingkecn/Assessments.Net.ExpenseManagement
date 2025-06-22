using System.ComponentModel.DataAnnotations;

namespace Assessments.ExpenseManagement.Command.Api.Models;

public sealed record AddEmployeeExpenseResponse([property: Required] Guid Id);
