namespace Assessments.ExpenseManagement.Application.Specifications;

public sealed class GetExpensesByUserIdOrderByAmountDescendingSpecification : GetExpensesByUserIdSpecification
{
    public GetExpensesByUserIdOrderByAmountDescendingSpecification(Guid userId) : base(userId) =>
        OrderBy = q => q.OrderByDescending(e => e.Amount).ThenByDescending(e => e.ExecutionDate);
}
