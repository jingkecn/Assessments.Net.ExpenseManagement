using Assessments.ExpenseManagement.Application.Specifications;
using Assessments.ExpenseManagement.Domain.Contracts;
using Assessments.ExpenseManagement.Domain.Models;
using Mediator;

namespace Assessments.ExpenseManagement.Application.Queries;

public sealed class GetCurrenciesQueryHandler(IReadRepository<Currency> currencyReadRepository)
    : IQueryHandler<GetCurrenciesQuery, IEnumerable<Currency>>
{
    public async ValueTask<IEnumerable<Currency>> Handle(GetCurrenciesQuery query, CancellationToken cancellationToken)
    {
        var specification = new GetCurrenciesSpecification();
        return await currencyReadRepository.GetAsync(specification, cancellationToken: cancellationToken);
    }
}
