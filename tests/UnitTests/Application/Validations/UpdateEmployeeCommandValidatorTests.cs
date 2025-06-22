using Assessments.ExpenseManagement.Application.Commands;
using Assessments.ExpenseManagement.Application.Validations;
using FluentValidation;
using Shouldly;

namespace Assessments.ExpenseManagement.UnitTests.Application.Validations;

public sealed class UpdateEmployeeCommandValidatorTests
{
    [Theory]
    [InlineData("")]
    [InlineData("AB")]
    [InlineData("ABCD")]
    public async Task Should_ThrowValidationException_When_CurrencyCodeIsInvalid(string currencyCode)
    {
        // Arrange
        var command = new UpdateEmployeeCommand(Guid.NewGuid(), currencyCode);
        var validator = new UpdateEmployeeCommandValidator();

        // Act
        var action = async () => await validator.ValidateAndThrowAsync(command);

        // Assert
        var exception = await action.ShouldThrowAsync<ValidationException>();
        exception.Errors.ShouldContain(e => e.PropertyName == nameof(command.CurrencyCode));
    }
}
