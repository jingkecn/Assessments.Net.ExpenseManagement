using Assessments.ExpenseManagement.Application.Queries;
using Assessments.ExpenseManagement.Application.Specifications;
using Assessments.ExpenseManagement.Infrastructure.Presets;
using Assessments.ExpenseManagement.UnitTests.Abstractions;
using NSubstitute;
using Shouldly;

namespace Assessments.ExpenseManagement.UnitTests.Application.Queries;

public sealed class GetCategoriesQueryHandlerTests : TestBase
{
    [Fact]
    public async Task Handle_Should_GetAllCategories_When_Successful()
    {
        // Arrange
        var query = new GetCategoriesQuery();
        var handler = new GetCategoriesQueryHandler(CategoryReadRepository);

        var expected = CategoryPresets.PredefinedCategories.ToList();
        CategoryReadRepository
            .GetAsync(Arg.Any<GetCategoriesSpecification>())
            .Returns(expected);

        // Act
        var results = await handler.Handle(query, CancellationToken.None);

        // Assert
        results.ShouldBe(expected, true);

        await CategoryReadRepository
            .Received(1)
            .GetAsync(Arg.Any<GetCategoriesSpecification>());
    }
}
