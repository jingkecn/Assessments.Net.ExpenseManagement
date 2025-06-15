namespace Assessments.ExpenseManagement.Application.Specifications;

public sealed class GetExpensesByUserIdOrderByExecutionDateAscendingSpecification : GetExpensesByUserIdSpecification
{
    public GetExpensesByUserIdOrderByExecutionDateAscendingSpecification(Guid userId) : base(userId) =>
        OrderBy = q => q.OrderBy(e => e.ExecutionDate).ThenBy(e => e.Amount);
}
