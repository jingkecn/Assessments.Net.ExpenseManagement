using Assessments.ExpenseManagement.Domain.Models;
using Assessments.ExpenseManagement.Infrastructure.Presets;
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
        var currency = CurrencyPresets.CNY;

        // Act
        var result = Employee.Create(firstName, lastName, currency);

        // Assert
        result.ShouldNotBeNull();
        result.Id.ShouldNotBe(Guid.Empty);
        result.FirstName.ShouldBe(firstName);
        result.LastName.ShouldBe(lastName);
        result.CurrencyId.ShouldBe(currency.Id);
    }

    [Fact]
    public void Create_Should_ReturnTransientEmployee_When_IsTransientIsTrue()
    {
        // Arrange

        const string firstName = "John";
        const string lastName = "Doe";
        var currency = CurrencyPresets.CNY;

        // Act
        var result = Employee.Create(firstName, lastName, currency, true);

        // Assert
        result.ShouldNotBeNull();
        result.Id.ShouldBe(Guid.Empty);
        result.FirstName.ShouldBe(firstName);
        result.LastName.ShouldBe(lastName);
        result.CurrencyId.ShouldBe(currency.Id);
    }

    [Fact]
    public void UpdateCurrency_Should_UpdateCurrency_WhenCurrencyIsSupported()
    {
        // Arrange
        const string firstName = "John";
        const string lastName = "Doe";
        var oldCurrency = CurrencyPresets.USD;
        var employee = Employee.Create(firstName, lastName, oldCurrency);
        var newCurrency = CurrencyPresets.CNY;

        // Act
        employee.UpdateCurrency(newCurrency);

        // Act & Assert
        employee.Id.ShouldNotBe(Guid.Empty);
        employee.FirstName.ShouldBe(firstName);
        employee.LastName.ShouldBe(lastName);
        employee.CurrencyId.ShouldBe(newCurrency.Id);
    }
}
