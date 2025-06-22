using Assessments.ExpenseManagement.Domain.Models;
using Mediator;

namespace Assessments.ExpenseManagement.Application.Queries;

public sealed record GetCurrenciesQuery : IQuery<IEnumerable<Currency>>;
