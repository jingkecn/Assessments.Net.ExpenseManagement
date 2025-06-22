using Assessments.ExpenseManagement.Application.Queries;
using Assessments.ExpenseManagement.Application.Specifications;
using Assessments.ExpenseManagement.Domain.Exceptions;
using Assessments.ExpenseManagement.Infrastructure.Presets;
using Assessments.ExpenseManagement.UnitTests.Abstractions;
using NSubstitute;
using Shouldly;

namespace Assessments.ExpenseManagement.UnitTests.Application.Queries;

public sealed class GetCurrencyByCodeQueryHandlerTests : TestBase
{
    [Fact]
    public async Task Handle_Should_GetCurrency_WhenSuccess()
    {
        // Arrange
        var expected = CurrencyPresets.CNY;
        var query = new GetCurrencyByCodeQuery(expected.Name);
        var handler = new GetCurrencyByCodeQueryHandler(CurrencyReadRepository);

        CurrencyReadRepository
            .ExistsAsync(Arg.Any<GetCurrencyByCodeSpecification>())
            .Returns(true);
        CurrencyReadRepository
            .GetAsync(Arg.Any<GetCurrencyByCodeSpecification>(), Arg.Any<int>(), Arg.Any<int>())
            .Returns([expected]);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.ShouldBe(expected);

        await CurrencyReadRepository
            .Received(1)
            .ExistsAsync(Arg.Any<GetCurrencyByCodeSpecification>());
        await CurrencyReadRepository
            .Received(1)
            .GetAsync(Arg.Any<GetCurrencyByCodeSpecification>(), Arg.Any<int>(), Arg.Any<int>());
    }

    [Fact]
    public async Task Handle_Should_ThrowDomainException_WhenCurrencyNotFound()
    {
        // Arrange
        var query = new GetCurrencyByCodeQuery("Whatever");
        var handler = new GetCurrencyByCodeQueryHandler(CurrencyReadRepository);

        CurrencyReadRepository
            .ExistsAsync(Arg.Any<GetCurrencyByCodeSpecification>())
            .Returns(false);

        // Act
        var action = async () => await handler.Handle(query, CancellationToken.None);

        // Assert
        var exception = await action.ShouldThrowAsync<DomainException>();
        var expected = DomainException.CurrencyNotFoundByCode(query.Code);
        exception.Message.ShouldBe(expected.Message);
        exception.StatusCode.ShouldBe(expected.StatusCode);

        await CurrencyReadRepository
            .Received(1)
            .ExistsAsync(Arg.Any<GetCurrencyByCodeSpecification>());
        await CurrencyReadRepository
            .Received(0)
            .GetAsync(Arg.Any<GetCurrencyByCodeSpecification>(), Arg.Any<int>(), Arg.Any<int>());
    }
}
