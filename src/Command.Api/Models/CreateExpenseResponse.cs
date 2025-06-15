using System.ComponentModel.DataAnnotations;

namespace Assessments.ExpenseManagement.Command.Api.Models;

public sealed record CreateExpenseResponse([property: Required] Guid Id);
