using Assessments.ExpenseManagement.Application.Specifications;
using Assessments.ExpenseManagement.Application.UseCases.Create;
using Assessments.ExpenseManagement.Application.Validations;
using Assessments.ExpenseManagement.Infrastructure.Models;
using Assessments.ExpenseManagement.UnitTests.Abstractions;
using FluentValidation;
using NSubstitute;
using Shouldly;

namespace Assessments.ExpenseManagement.UnitTests.Application.Validations;

public sealed class CreateExpenseCommandValidatorTests : TestBase
{
    [Fact]
    public async Task Should_ThrowValidationException_When_CommentIsEmpty()
    {
        // Arrange
        var category = CategoryPresets.Hotel;
        var currency = CurrencyPresets.USD;
        var user = UserPresets.JohnDoe;
        user.Currency = currency;
        var command = new CreateExpenseCommand(
            100M,
            category.Name,
            currency.Code,
            string.Empty, // Empty description
            DateOnly.FromDateTime(DateTime.Now),
            user.Id);

        UserReadRepository
            .GetAsync(Arg.Any<GetUserByIdSpecification>())
            .Returns([user]);

        var validator = new CreateExpenseCommandValidator(ExpenseReadRepository, UserReadRepository);

        // Act
        var action = async () => await validator.ValidateAndThrowAsync(command);

        // Assert
        var exception = await action.ShouldThrowAsync<ValidationException>();
        exception.Errors.ShouldContain(e => e.PropertyName == nameof(command.Description));
    }

    [Fact]
    public async Task Should_ThrowValidationException_When_ExecutionDateIsDatedMoreThanThreeMonths()
    {
        // Arrange
        var category = CategoryPresets.Hotel;
        var currency = CurrencyPresets.USD;
        var user = UserPresets.JohnDoe;
        user.Currency = currency;
        var command = new CreateExpenseCommand(
            100M,
            category.Name,
            "Comment",
            currency.Code,
            DateOnly.FromDateTime(DateTime.Now.AddMonths(-3).AddDays(-1)), // Invalid execution date
            user.Id);

        UserReadRepository
            .GetAsync(Arg.Any<GetUserByIdSpecification>())
            .Returns([user]);

        var validator = new CreateExpenseCommandValidator(ExpenseReadRepository, UserReadRepository);

        // Act
        var action = async () => await validator.ValidateAndThrowAsync(command);

        // Assert
        var exception = await action.ShouldThrowAsync<ValidationException>();
        exception.Errors.ShouldContain(e => e.PropertyName == nameof(command.ExecutionDate));
    }

    [Fact]
    public async Task Should_ThrowValidationException_When_ExecutionDateIsFutureDate()
    {
        // Arrange
        var category = CategoryPresets.Hotel;
        var currency = CurrencyPresets.USD;
        var user = UserPresets.JohnDoe;
        user.Currency = currency;
        var command = new CreateExpenseCommand(
            100M,
            category.Name,
            "Comment",
            currency.Code,
            DateOnly.FromDateTime(DateTime.Now.AddDays(1)), // Invalid execution date
            user.Id);

        UserReadRepository
            .GetAsync(Arg.Any<GetUserByIdSpecification>())
            .Returns([user]);

        var validator = new CreateExpenseCommandValidator(ExpenseReadRepository, UserReadRepository);

        // Act
        var action = async () => await validator.ValidateAndThrowAsync(command);

        // Assert
        var exception = await action.ShouldThrowAsync<ValidationException>();
        exception.Errors.ShouldContain(e => e.PropertyName == nameof(command.ExecutionDate));
    }

    [Fact]
    public async Task Should_ThrowValidationException_When_ExpenseCurrencyIsNotAlignedWithUserCurrency()
    {
        // Arrange
        var category = CategoryPresets.Hotel;
        var currency = CurrencyPresets.USD;
        var user = UserPresets.JohnDoe;
        user.Currency = currency;
        var command = new CreateExpenseCommand(
            100M,
            category.Name,
            "Comment",
            CurrencyPresets.EUR.Code, // Invalid currency
            DateOnly.FromDateTime(DateTime.Now),
            user.Id);

        ExpenseReadRepository
            .ExistsAsync(Arg.Any<GetExpenseByUserIdAndAmountAndExecutionDate>())
            .Returns(true);

        UserReadRepository
            .GetAsync(Arg.Any<GetUserByIdSpecification>())
            .Returns([user]);

        var validator = new CreateExpenseCommandValidator(ExpenseReadRepository, UserReadRepository);

        // Act
        var action = async () => await validator.ValidateAndThrowAsync(command);

        // Assert
        var exception = await action.ShouldThrowAsync<ValidationException>();
        exception.Errors.ShouldNotBeEmpty();
    }

    [Fact]
    public async Task Should_ThrowValidationException_When_ExpenseIsDuplicated()
    {
        // Arrange
        var category = CategoryPresets.Hotel;
        var currency = CurrencyPresets.USD;
        var user = UserPresets.JohnDoe;
        user.Currency = currency;
        var command = new CreateExpenseCommand(
            100M,
            category.Name,
            "Comment",
            currency.Code,
            DateOnly.FromDateTime(DateTime.Now),
            user.Id);

        ExpenseReadRepository
            .ExistsAsync(Arg.Any<GetExpenseByUserIdAndAmountAndExecutionDate>())
            .Returns(true);

        UserReadRepository
            .GetAsync(Arg.Any<GetUserByIdSpecification>())
            .Returns([user]);

        var validator = new CreateExpenseCommandValidator(ExpenseReadRepository, UserReadRepository);

        // Act
        var action = async () => await validator.ValidateAndThrowAsync(command);

        // Assert
        var exception = await action.ShouldThrowAsync<ValidationException>();
        exception.Errors.ShouldNotBeEmpty();
    }
}
