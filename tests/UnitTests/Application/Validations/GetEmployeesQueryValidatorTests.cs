using Assessments.ExpenseManagement.Application.Queries;
using Assessments.ExpenseManagement.Application.Validations;
using FluentValidation;
using Shouldly;

namespace Assessments.ExpenseManagement.UnitTests.Application.Validations;

public sealed class GetEmployeesQueryValidatorTests
{
    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public async Task Should_ThrowValidationException_When_PageNumberIsInvalid(int pageNumber)
    {
        // Arrange
        var query = new GetEmployeesQuery(pageNumber, 1);
        var validator = new GetEmployeesQueryValidator();

        // Act
        var action = async () => await validator.ValidateAndThrowAsync(query);

        // Assert
        var exception = await action.ShouldThrowAsync<ValidationException>();
        exception.Errors.ShouldContain(e => e.PropertyName == nameof(GetEmployeesQuery.PageNumber));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public async Task Should_ThrowValidationException_When_PageSizeIsInvalid(int pageSize)
    {
        // Arrange
        var query = new GetEmployeesQuery(1, pageSize);
        var validator = new GetEmployeesQueryValidator();

        // Act
        var action = async () => await validator.ValidateAndThrowAsync(query);

        // Assert
        var exception = await action.ShouldThrowAsync<ValidationException>();
        exception.Errors.ShouldContain(e => e.PropertyName == nameof(GetEmployeesQuery.PageSize));
    }
}
