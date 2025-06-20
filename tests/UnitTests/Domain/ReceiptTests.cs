using Assessments.ExpenseManagement.Domain.Models;
using Shouldly;

namespace Assessments.ExpenseManagement.UnitTests.Domain;

public sealed class ReceiptTests
{
    [Fact]
    public void Create_Should_ReturnReceipt()
    {
        // Arrange
        const string comment = "Test Receipt";
        const string fileUrl = "receipt.jpg";

        // Act
        var result = Receipt.Create(comment, fileUrl);

        // Assert
        result.Id.ShouldNotBe(Guid.Empty);
        result.Comment.ShouldBe(comment);
        result.FileUrl.ShouldBe(fileUrl);
        result.Exists.ShouldBeTrue();

        result.DomainEvents.ShouldBeEmpty();
    }

    [Fact]
    public void Create_Should_ReturnTransientReceipt_WhenSetTransient()
    {
        // Arrange
        const string comment = "Test Receipt";
        const string fileUrl = "receipt.jpg";

        // Act
        var result = Receipt.Create(comment, fileUrl, true);

        // Assert
        result.Id.ShouldBe(Guid.Empty);
        result.Comment.ShouldBe(comment);
        result.FileUrl.ShouldBe(fileUrl);
        result.Exists.ShouldBeTrue();

        result.DomainEvents.ShouldBeEmpty();
    }

    [Fact]
    public void Exists_ShouldReturnFalse_WhenReceiptDoesNotExist()
    {
        // Arrange
        var receipt = Receipt.Create("Nonexistent Receipt", string.Empty);

        // Act
        var result = receipt.Exists;

        // Assert
        result.ShouldBeFalse();
    }

    [Fact]
    public void Exists_ShouldReturnTrue_WhenReceiptExists()
    {
        // Arrange
        var receipt = Receipt.Create("Existing Receipt", "receipt.jpg");

        // Act
        var result = receipt.Exists;

        // Assert
        result.ShouldBeTrue();
    }
}
