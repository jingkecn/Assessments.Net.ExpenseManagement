using System.ComponentModel.DataAnnotations;

namespace Assessments.ExpenseManagement.Command.Api.Models;

public sealed record PatchEmployeeExpenseRequest([property: Required] string Action);
