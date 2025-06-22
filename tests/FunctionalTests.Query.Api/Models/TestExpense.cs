namespace Assessments.ExpenseManagement.FunctionalTests.Query.Api.Models;

public sealed record TestExpense
{
    public decimal Amount { get; set; }
    public string Employee { get; set; } = null!;
    public DateOnly ExecutionDate { get; set; }
    public string Status { get; set; } = null!;
    public DateOnly SubmissionDate { get; set; }
}
