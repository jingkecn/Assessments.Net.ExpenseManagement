using Assessments.ExpenseManagement.Application.Commands;
using Assessments.ExpenseManagement.Application.Validations;
using FluentValidation;
using Shouldly;

namespace Assessments.ExpenseManagement.UnitTests.Application.Validations;

public sealed class PatchEmployeeExpenseCommandValidatorTests
{
    [Theory]
    [InlineData("")]
    [InlineData("copy")]
    public async Task Should_ThrowValidationException_When_ActionIsInvalid(string action)
    {
        // Arrange
        var command = new PatchEmployeeExpenseCommand(action, Guid.NewGuid(), Guid.NewGuid());
        var validator = new PatchEmployeeExpenseCommandValidator();

        // Act
        var act = async () => await validator.ValidateAndThrowAsync(command);

        // Assert
        var exception = await act.ShouldThrowAsync<ValidationException>();
        exception.Errors.ShouldContain(e => e.PropertyName == nameof(command.Action));
    }
}
