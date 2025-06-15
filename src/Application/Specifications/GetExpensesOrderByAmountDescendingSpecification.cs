namespace Assessments.ExpenseManagement.Application.Specifications;

public sealed class GetExpensesOrderByAmountDescendingSpecification : GetExpensesSpecification
{
    public GetExpensesOrderByAmountDescendingSpecification() =>
        OrderBy = q => q.OrderByDescending(e => e.Amount).ThenByDescending(e => e.ExecutionDate);
}
