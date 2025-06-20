using Assessments.ExpenseManagement.Domain.Models;
using Shouldly;

namespace Assessments.ExpenseManagement.UnitTests.Domain;

public sealed class AmountTests
{
    [Fact]
    public void IsValid_Should_ReturnFalse_When_ValueIsNegative()
    {
        // Arrange
        var amount = new Amount { Value = -50.00m };

        // Act
        var result = amount.IsValid;

        // Assert
        result.ShouldBeFalse();
    }

    [Fact]
    public void IsValid_Should_ReturnFalse_When_ValueIsZero()
    {
        // Arrange
        var amount = new Amount { Value = 0.00m };

        // Act
        var result = amount.IsValid;

        // Assert
        result.ShouldBeFalse();
    }

    [Fact]
    public void IsValid_Should_ReturnTrue_When_ValueIsPositive()
    {
        // Arrange
        var amount = new Amount { Value = 100.00m };

        // Act
        var result = amount.IsValid;

        // Assert
        result.ShouldBeTrue();
    }
}
