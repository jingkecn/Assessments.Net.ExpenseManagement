namespace Assessments.ExpenseManagement.Application.Specifications;

public sealed class GetExpensesOrderByExecutionDateDescendingSpecification : GetExpensesSpecification
{
    public GetExpensesOrderByExecutionDateDescendingSpecification() =>
        OrderBy = q => q.OrderByDescending(e => e.ExecutionDate).ThenBy(e => e.Amount);
}
