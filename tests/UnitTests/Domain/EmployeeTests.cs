using Assessments.ExpenseManagement.Domain.Models;
using Shouldly;

namespace Assessments.ExpenseManagement.UnitTests.Domain;

public sealed class EmployeeTests
{
    [Fact]
    public void Create_Should_ReturnEmployee()
    {
        // Arrange

        const string firstName = "John";
        const string lastName = "Doe";
        const string currencyCode = "CNY";

        // Act
        var result = Employee.Create(firstName, lastName, currencyCode);

        // Assert
        result.ShouldNotBeNull();
        result.Id.ShouldNotBe(Guid.Empty);
        result.FirstName.ShouldBe(firstName);
        result.LastName.ShouldBe(lastName);
        result.CurrencyCode.ShouldBe(currencyCode);
    }

    [Fact]
    public void Create_Should_ReturnTransientEmployee_When_IsTransientIsTrue()
    {
        // Arrange

        const string firstName = "John";
        const string lastName = "Doe";
        const string currencyCode = "CNY";

        // Act
        var result = Employee.Create(firstName, lastName, currencyCode, true);

        // Assert
        result.ShouldNotBeNull();
        result.Id.ShouldBe(Guid.Empty);
        result.FirstName.ShouldBe(firstName);
        result.LastName.ShouldBe(lastName);
        result.CurrencyCode.ShouldBe(currencyCode);
    }

    [Fact]
    public void UpdateCurrency_Should_UpdateCurrency_WhenCurrencyIsSupported()
    {
        // Arrange
        const string firstName = "John";
        const string lastName = "Doe";
        const string oldCurrencyCode = "USD";
        var employee = Employee.Create(firstName, lastName, oldCurrencyCode);
        const string newCurrencyCode = "CNY";

        // Act
        employee.UpdateCurrencyCode(newCurrencyCode);

        // Act & Assert
        employee.Id.ShouldNotBe(Guid.Empty);
        employee.FirstName.ShouldBe(firstName);
        employee.LastName.ShouldBe(lastName);
        employee.CurrencyCode.ShouldBe(newCurrencyCode);
    }
}
