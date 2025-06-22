using Assessments.ExpenseManagement.Application.Commands;
using Assessments.ExpenseManagement.Application.Models;
using Assessments.ExpenseManagement.Application.Validations;
using Assessments.ExpenseManagement.Infrastructure.Presets;
using FluentValidation;
using Shouldly;

namespace Assessments.ExpenseManagement.UnitTests.Application.Validations;

public sealed class AddEmployeeExpenseCommandValidatorTests
{
    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public async Task Should_ThrowValidationException_When_AmountValueIsInvalid(decimal amountValue)
    {
        // Arrange
        var category = CategoryPresets.Hotel;
        var currency = CurrencyPresets.CNY;
        var employee = EmployeePresets.JohnDoe;
        var command = new AddEmployeeExpenseCommand(
            amountValue,
            category.Name,
            currency.Code,
            employee.Id,
            DateOnly.FromDateTime(DateTime.Now),
            [new ReceiptView("No comment", "receipt.jpg")]);
        var validator = new AddEmployeeExpenseCommandValidator();

        // Act
        var action = async () => await validator.ValidateAndThrowAsync(command);

        // Assert
        var exception = await action.ShouldThrowAsync<ValidationException>();
        exception.Errors.ShouldContain(e => e.PropertyName == nameof(command.Amount));
    }

    [Theory]
    [InlineData("")]
    [InlineData("0123456789|0123456789|0123456789|0123456789|0123456789")]
    public async Task Should_ThrowValidationException_When_CategoryNameIsInvalid(string categoryName)
    {
        // Arrange
        var currency = CurrencyPresets.CNY;
        var employee = EmployeePresets.JohnDoe;
        var command = new AddEmployeeExpenseCommand(
            100M,
            categoryName,
            currency.Code,
            employee.Id,
            DateOnly.FromDateTime(DateTime.Now),
            [new ReceiptView("No comment", "receipt.jpg")]);
        var validator = new AddEmployeeExpenseCommandValidator();

        // Act
        var action = async () => await validator.ValidateAndThrowAsync(command);

        // Assert
        var exception = await action.ShouldThrowAsync<ValidationException>();
        exception.Errors.ShouldContain(e => e.PropertyName == nameof(command.CategoryName));
    }

    [Theory]
    [InlineData("")]
    [InlineData("AB")]
    [InlineData("ABCD")]
    public async Task Should_ThrowValidationException_When_CurrencyCodeIsInvalid(string currencyCode)
    {
        // Arrange
        var category = CategoryPresets.Hotel;
        var employee = EmployeePresets.JohnDoe;
        var command = new AddEmployeeExpenseCommand(
            100M,
            category.Name,
            currencyCode,
            employee.Id,
            DateOnly.FromDateTime(DateTime.Now),
            [new ReceiptView("No comment", "receipt.jpg")]);
        var validator = new AddEmployeeExpenseCommandValidator();

        // Act
        var action = async () => await validator.ValidateAndThrowAsync(command);

        // Assert
        var exception = await action.ShouldThrowAsync<ValidationException>();
        exception.Errors.ShouldContain(e => e.PropertyName == nameof(command.CurrencyCode));
    }

    [Theory]
    [ClassData(typeof(InvalidExecutionDateGenerator))]
    public async Task Should_ThrowValidationException_When_ExecutionDateIsInvalid(DateOnly executionDate)
    {
        // Arrange
        var category = CategoryPresets.Hotel;
        var currency = CurrencyPresets.CNY;
        var employee = EmployeePresets.JohnDoe;
        var command = new AddEmployeeExpenseCommand(
            100M,
            category.Name,
            currency.Code,
            employee.Id,
            executionDate,
            [new ReceiptView("No comment", "receipt.jpg")]);
        var validator = new AddEmployeeExpenseCommandValidator();

        // Act
        var action = async () => await validator.ValidateAndThrowAsync(command);

        // Assert
        var exception = await action.ShouldThrowAsync<ValidationException>();
        exception.Errors.ShouldContain(e => e.PropertyName == nameof(command.ExecutionDate));
    }

    [Fact]
    public async Task Should_ThrowValidationException_When_ReceiptListIsInvalid()
    {
        // Arrange
        var category = CategoryPresets.Hotel;
        var currency = CurrencyPresets.CNY;
        var employee = EmployeePresets.JaneDoe;
        var command = new AddEmployeeExpenseCommand(
            100M,
            category.Name,
            currency.Code,
            employee.Id,
            DateOnly.FromDateTime(DateTime.Now),
            []);
        var validator = new AddEmployeeExpenseCommandValidator();

        // Act
        var action = async () => await validator.ValidateAndThrowAsync(command);

        // Assert
        var exception = await action.ShouldThrowAsync<ValidationException>();
        exception.Errors.ShouldContain(e => e.PropertyName == nameof(command.Receipts));
    }

    private sealed class InvalidExecutionDateGenerator : TheoryData<DateOnly>
    {
        public InvalidExecutionDateGenerator() => Add(DateOnly.FromDateTime(DateTime.Now.AddDays(1)));
    }
}
