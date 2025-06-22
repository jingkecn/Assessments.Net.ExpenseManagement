using Assessments.ExpenseManagement.Application.Queries;
using Assessments.ExpenseManagement.Application.Specifications;
using Assessments.ExpenseManagement.Infrastructure.Presets;
using Assessments.ExpenseManagement.UnitTests.Abstractions;
using NSubstitute;
using Shouldly;

namespace Assessments.ExpenseManagement.UnitTests.Application.Queries;

public sealed class GetCurrenciesQueryHandlerTests : TestBase
{
    [Fact]
    public async Task Handle_Should_GetAllCurrencies_When_Successful()
    {
        // Arrange
        var query = new GetCurrenciesQuery();
        var handler = new GetCurrenciesQueryHandler(CurrencyReadRepository);

        var expected = CurrencyPresets.PredefinedCurrencies.ToList();
        CurrencyReadRepository
            .GetAsync(Arg.Any<GetCurrenciesSpecification>())
            .Returns(expected);

        // Act
        var results = await handler.Handle(query, CancellationToken.None);

        // Assert
        results.ShouldBe(expected, true);

        await CurrencyReadRepository
            .Received(1)
            .GetAsync(Arg.Any<GetCurrenciesSpecification>());
    }
}
