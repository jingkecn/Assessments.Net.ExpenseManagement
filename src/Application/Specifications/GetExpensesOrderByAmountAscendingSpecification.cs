namespace Assessments.ExpenseManagement.Application.Specifications;

public sealed class GetExpensesOrderByAmountAscendingSpecification : GetExpensesSpecification
{
    public GetExpensesOrderByAmountAscendingSpecification() =>
        OrderBy = q => q.OrderBy(e => e.Amount).ThenByDescending(e => e.ExecutionDate);
}
