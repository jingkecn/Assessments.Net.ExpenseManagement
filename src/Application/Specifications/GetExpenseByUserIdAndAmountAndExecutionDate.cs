using Assessments.ExpenseManagement.Application.Abstractions;
using Assessments.ExpenseManagement.Domain.Models;

namespace Assessments.ExpenseManagement.Application.Specifications;

public sealed class GetExpenseByUserIdAndAmountAndExecutionDate(Guid userId, decimal amount, DateOnly executionDate)
    : Specification<Expense>(e => e.UserId == userId && e.Amount == amount && e.ExecutionDate == executionDate);
