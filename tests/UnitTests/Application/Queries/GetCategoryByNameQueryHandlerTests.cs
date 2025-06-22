using Assessments.ExpenseManagement.Application.Queries;
using Assessments.ExpenseManagement.Application.Specifications;
using Assessments.ExpenseManagement.Domain.Exceptions;
using Assessments.ExpenseManagement.Infrastructure.Presets;
using Assessments.ExpenseManagement.UnitTests.Abstractions;
using NSubstitute;
using Shouldly;

namespace Assessments.ExpenseManagement.UnitTests.Application.Queries;

public sealed class GetCategoryByNameQueryHandlerTests : TestBase
{
    [Fact]
    public async Task Handle_Should_GetCategory_WhenSuccess()
    {
        // Arrange
        var expected = CategoryPresets.Hotel;
        var query = new GetCategoryByNameQuery(expected.Name);
        var handler = new GetCategoryByNameQueryHandler(CategoryReadRepository);

        CategoryReadRepository
            .ExistsAsync(Arg.Any<GetCategoryByNameSpecification>())
            .Returns(true);
        CategoryReadRepository
            .GetAsync(Arg.Any<GetCategoryByNameSpecification>(), Arg.Any<int>(), Arg.Any<int>())
            .Returns([expected]);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.ShouldBe(expected);

        await CategoryReadRepository
            .Received(1)
            .ExistsAsync(Arg.Any<GetCategoryByNameSpecification>());
        await CategoryReadRepository
            .Received(1)
            .GetAsync(Arg.Any<GetCategoryByNameSpecification>(), Arg.Any<int>(), Arg.Any<int>());
    }

    [Fact]
    public async Task Handle_Should_ThrowDomainException_WhenCategoryNotFound()
    {
        // Arrange
        var query = new GetCategoryByNameQuery("Whatever");
        var handler = new GetCategoryByNameQueryHandler(CategoryReadRepository);

        CategoryReadRepository
            .ExistsAsync(Arg.Any<GetCategoryByNameSpecification>())
            .Returns(false);

        // Act
        var action = async () => await handler.Handle(query, CancellationToken.None);

        // Assert
        var exception = await action.ShouldThrowAsync<DomainException>();
        var expected = DomainException.CategoryNotFoundByName(query.Name);
        exception.Message.ShouldBe(expected.Message);
        exception.StatusCode.ShouldBe(expected.StatusCode);

        await CategoryReadRepository
            .Received(1)
            .ExistsAsync(Arg.Any<GetCategoryByNameSpecification>());
        await CategoryReadRepository
            .Received(0)
            .GetAsync(Arg.Any<GetCategoryByNameSpecification>(), Arg.Any<int>(), Arg.Any<int>());
    }
}
