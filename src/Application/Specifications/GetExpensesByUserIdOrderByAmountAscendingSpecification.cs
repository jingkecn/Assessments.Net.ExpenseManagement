namespace Assessments.ExpenseManagement.Application.Specifications;

public sealed class GetExpensesByUserIdOrderByAmountAscendingSpecification : GetExpensesByUserIdSpecification
{
    public GetExpensesByUserIdOrderByAmountAscendingSpecification(Guid userId) : base(userId) =>
        OrderBy = q => q.OrderBy(e => e.Amount).ThenByDescending(e => e.ExecutionDate);
}
