namespace Assessments.ExpenseManagement.Application.Specifications;

public sealed class GetExpensesOrderByExecutionDateAscendingSpecification : GetExpensesSpecification
{
    public GetExpensesOrderByExecutionDateAscendingSpecification() =>
        OrderBy = q => q.OrderBy(e => e.ExecutionDate).ThenBy(e => e.Amount);
}
