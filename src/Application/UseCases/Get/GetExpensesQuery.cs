using System.ComponentModel.DataAnnotations;
using Assessments.ExpenseManagement.Application.Models;
using Mediator;

namespace Assessments.ExpenseManagement.Application.UseCases.Get;

public record GetExpensesQuery(
    [property: Required] SortBy SortBy,
    [property: Required] SortDirection SortDirection) : IQuery<IEnumerable<ExpenseView>>;
