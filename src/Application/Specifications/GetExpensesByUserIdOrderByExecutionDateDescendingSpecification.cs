namespace Assessments.ExpenseManagement.Application.Specifications;

public sealed class GetExpensesByUserIdOrderByExecutionDateDescendingSpecification : GetExpensesByUserIdSpecification
{
    public GetExpensesByUserIdOrderByExecutionDateDescendingSpecification(Guid userId) : base(userId) =>
        OrderBy = q => q.OrderByDescending(e => e.ExecutionDate).ThenBy(e => e.Amount);
}
