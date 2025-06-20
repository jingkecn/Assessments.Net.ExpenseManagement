using Assessments.ExpenseManagement.Domain.Models;
using Shouldly;

namespace Assessments.ExpenseManagement.UnitTests.Domain;

public sealed class CurrencyTests
{
    [Fact]
    public void Create_Should_ReturnCurrency()
    {
        // Arrange
        const string code = "CNY";
        const string name = "Chinese Yuan";
        const string symbol = "¥";

        // Act
        var currency = Currency.Create(code, name, symbol);

        // Assert
        currency.ShouldNotBeNull();
        currency.Id.ShouldNotBe(Guid.Empty);
        currency.Code.ShouldBe(code);
        currency.Name.ShouldBe(name);
        currency.Symbol.ShouldBe(symbol);
    }

    [Fact]
    public void Create_Should_ReturnTransientCurrency_When_IsTransientIsTrue()
    {
        // Arrange
        const string code = "CNY";
        const string name = "Chinese Yuan";
        const string symbol = "¥";

        // Act
        var currency = Currency.Create(code, name, symbol, true);

        // Assert
        currency.ShouldNotBeNull();
        currency.Id.ShouldBe(Guid.Empty);
        currency.Code.ShouldBe(code);
        currency.Name.ShouldBe(name);
        currency.Symbol.ShouldBe(symbol);
    }
}
