using Assessments.ExpenseManagement.Application.Specifications;
using Assessments.ExpenseManagement.Domain.Contracts;
using Assessments.ExpenseManagement.Domain.Exceptions;
using Assessments.ExpenseManagement.Domain.Models;
using Mediator;

namespace Assessments.ExpenseManagement.Application.Queries;

public sealed class GetCurrencyByCodeQueryHandler(IReadRepository<Currency> currencyReadRepository)
    : IQueryHandler<GetCurrencyByCodeQuery, Currency>
{
    public async ValueTask<Currency> Handle(GetCurrencyByCodeQuery query, CancellationToken cancellationToken)
    {
        var specification = new GetCurrencyByCodeSpecification(query.Code);
        if (!await currencyReadRepository.ExistsAsync(specification, cancellationToken))
        {
            throw DomainException.CurrencyNotFoundByCode(query.Code);
        }

        var results = await currencyReadRepository.GetAsync(specification, cancellationToken: cancellationToken);
        return results.Single();
    }
}
