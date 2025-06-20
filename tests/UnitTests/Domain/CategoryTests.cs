using Assessments.ExpenseManagement.Domain.Models;
using Shouldly;

namespace Assessments.ExpenseManagement.UnitTests.Domain;

public sealed class CategoryTests
{
    [Fact]
    public void Create_Should_ReturnCategory()
    {
        // Arrange
        const string name = "Travel";

        // Act
        var category = Category.Create(name);

        // Assert
        category.ShouldNotBeNull();
        category.Id.ShouldNotBe(Guid.Empty);
        category.Name.ShouldBe(name);
    }

    [Fact]
    public void Create_Should_ReturnTransientCategory_When_IsTransientIsTrue()
    {
        // Arrange
        const string name = "Travel";

        // Act
        var category = Category.Create(name, true);

        // Assert
        category.ShouldNotBeNull();
        category.Id.ShouldBe(Guid.Empty);
        category.Name.ShouldBe(name);
    }
}
