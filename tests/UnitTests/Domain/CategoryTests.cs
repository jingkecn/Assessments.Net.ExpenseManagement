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

    [Fact]
    public void IsAllowedFor_Should_ReturnFalse_WhenEmployeeIsNotAllowed()
    {
        // Arrange
        var category = Category.Create("Travel");
        var employee = Employee.Create("John", "Doe", "CNY", true);

        // Act
        var result = category.IsAllowedFor(employee);

        // Assert
        result.ShouldBeFalse();
    }

    [Fact]
    public void IsAllowedFor_Should_ReturnTrue_WhenEmployeeIsAllowed()
    {
        // Arrange
        var category = Category.Create("Travel");
        var employee = Employee.Create("John", "Doe", "CNY");

        // Act
        var result = category.IsAllowedFor(employee);

        // Assert
        result.ShouldBeTrue();
    }
}
