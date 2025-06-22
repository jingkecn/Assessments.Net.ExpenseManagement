using Assessments.ExpenseManagement.Application.Specifications;
using Assessments.ExpenseManagement.Domain.Contracts;
using Assessments.ExpenseManagement.Domain.Exceptions;
using Assessments.ExpenseManagement.Domain.Models;
using Mediator;

namespace Assessments.ExpenseManagement.Application.Queries;

public sealed class GetCategoryByNameQueryHandler(IReadRepository<Category> categoryReadRepository)
    : IQueryHandler<GetCategoryByNameQuery, Category>
{
    public async ValueTask<Category> Handle(GetCategoryByNameQuery query, CancellationToken cancellationToken)
    {
        var specification = new GetCategoryByNameSpecification(query.Name);
        if (!await categoryReadRepository.ExistsAsync(specification, cancellationToken))
        {
            throw DomainException.CategoryNotFoundByName(query.Name);
        }

        var results = await categoryReadRepository.GetAsync(specification, cancellationToken: cancellationToken);
        return results.Single();
    }
}
