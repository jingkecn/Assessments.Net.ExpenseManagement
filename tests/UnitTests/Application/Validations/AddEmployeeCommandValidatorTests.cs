using Assessments.ExpenseManagement.Application.Commands;
using Assessments.ExpenseManagement.Application.Validations;
using FluentValidation;
using Shouldly;

namespace Assessments.ExpenseManagement.UnitTests.Application.Validations;

public sealed class AddEmployeeCommandValidatorTests
{
    [Theory]
    [InlineData("")]
    [InlineData("AB")]
    [InlineData("ABCD")]
    public async Task Should_ThrowValidationException_When_CurrencyCodeIsInvalid(string currencyCode)
    {
        // Arrange
        var command = new AddEmployeeCommand("John", "Doe", currencyCode);
        var validator = new AddEmployeeCommandValidator();

        // Act
        var action = async () => await validator.ValidateAndThrowAsync(command);

        // Assert
        var exception = await action.ShouldThrowAsync<ValidationException>();
        exception.Errors.ShouldContain(e => e.PropertyName == nameof(command.CurrencyCode));
    }

    [Theory]
    [InlineData("")]
    [InlineData("0123456789|0123456789|0123456789|0123456789|0123456789")]
    public async Task Should_ThrowValidationException_When_FirstNameIsInvalid(string firstName)
    {
        // Arrange
        var command = new AddEmployeeCommand(firstName, "Doe", "CNY");
        var validator = new AddEmployeeCommandValidator();

        // Act
        var action = async () => await validator.ValidateAndThrowAsync(command);

        // Assert
        var exception = await action.ShouldThrowAsync<ValidationException>();
        exception.Errors.ShouldContain(e => e.PropertyName == nameof(command.FirstName));
    }

    [Theory]
    [InlineData("")]
    [InlineData("0123456789|0123456789|0123456789|0123456789|0123456789")]
    public async Task Should_ThrowValidationException_When_LastNameIsInvalid(string lastName)
    {
        // Arrange
        var command = new AddEmployeeCommand("John", lastName, "CNY");
        var validator = new AddEmployeeCommandValidator();

        // Act
        var action = async () => await validator.ValidateAndThrowAsync(command);

        // Assert
        var exception = await action.ShouldThrowAsync<ValidationException>();
        exception.Errors.ShouldContain(e => e.PropertyName == nameof(command.LastName));
    }
}
