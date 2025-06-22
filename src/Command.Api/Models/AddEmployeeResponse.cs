using System.ComponentModel.DataAnnotations;

namespace Assessments.ExpenseManagement.Command.Api.Models;

public sealed record AddEmployeeResponse([property: Required] Guid Id);
