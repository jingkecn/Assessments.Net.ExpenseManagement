using System.ComponentModel.DataAnnotations;

namespace Assessments.ExpenseManagement.Application.Models;

public sealed record ReceiptView(
    [property: Required] string Comment,
    [property: Required] string FileUrl);
