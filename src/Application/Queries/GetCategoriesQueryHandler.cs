using Assessments.ExpenseManagement.Application.Specifications;
using Assessments.ExpenseManagement.Domain.Contracts;
using Assessments.ExpenseManagement.Domain.Models;
using Mediator;

namespace Assessments.ExpenseManagement.Application.Queries;

public sealed class GetCategoriesQueryHandler(IReadRepository<Category> categoryReadRepository)
    : IQueryHandler<GetCategoriesQuery, IEnumerable<Category>>
{
    public async ValueTask<IEnumerable<Category>> Handle(GetCategoriesQuery query, CancellationToken cancellationToken)
    {
        var specification = new GetCategoriesSpecification();
        return await categoryReadRepository.GetAsync(specification, cancellationToken: cancellationToken);
    }
}
