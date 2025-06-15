using Assessments.ExpenseManagement.Application.Specifications;
using Assessments.ExpenseManagement.Application.UseCases.Create;
using Assessments.ExpenseManagement.Application.Validations;
using Assessments.ExpenseManagement.Domain.Exceptions;
using Assessments.ExpenseManagement.Domain.Models;
using Assessments.ExpenseManagement.Infrastructure.Models;
using Assessments.ExpenseManagement.UnitTests.Abstractions;
using NSubstitute;
using Shouldly;

namespace Assessments.ExpenseManagement.UnitTests.Application.UseCases;

public sealed class CreateExpenseCommandHandlerTests : TestBase
{
    [Fact]
    public async Task Handle_Should_ReturnExpenseId_When_Successful()
    {
        // Arrange
        var category = CategoryPresets.Hotel;
        var currency = CurrencyPresets.USD;
        var user = UserPresets.JohnDoe;
        user.Currency = currency;
        var expense = Expense.Create(
            100M,
            category,
            currency,
            "Test comment",
            DateOnly.FromDateTime(DateTime.Now),
            user);
        var command = new CreateExpenseCommand(
            100M,
            category.Name,
            currency.Code,
            nameof(CreateExpenseCommand.Description),
            DateOnly.FromDateTime(DateTime.Now),
            user.Id);
        var validator = new CreateExpenseCommandValidator(
            ExpenseReadRepository,
            UserReadRepository);
        var handler = new CreateExpenseCommandHandler(
            CategoryReadRepository,
            CurrencyReadRepository,
            ExpenseWriteRepository,
            UserReadRepository,
            validator);

        CategoryReadRepository
            .ExistsAsync(Arg.Any<GetCategoryByNameSpecification>())
            .Returns(true);
        CategoryReadRepository
            .GetAsync(Arg.Any<GetCategoryByNameSpecification>())
            .Returns([category]);

        CurrencyReadRepository
            .ExistsAsync(Arg.Any<GetCurrencyByCodeSpecification>())
            .Returns(true);
        CurrencyReadRepository
            .GetAsync(Arg.Any<GetCurrencyByCodeSpecification>())
            .Returns([currency]);

        ExpenseReadRepository
            .ExistsAsync(Arg.Any<GetExpenseByUserIdAndAmountAndExecutionDate>())
            .Returns(false);
        ExpenseWriteRepository
            .AddAsync(Arg.Any<Expense>())
            .Returns(expense);

        UserReadRepository
            .ExistsAsync(Arg.Any<GetUserByIdSpecification>())
            .Returns(true);
        UserReadRepository
            .GetAsync(Arg.Any<GetUserByIdSpecification>())
            .Returns([user]);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.ShouldBe(expense.Id);

        await ExpenseWriteRepository
            .Received(1)
            .AddAsync(Arg.Any<Expense>());
        await ExpenseWriteRepository
            .Received(1)
            .SaveChangesAsync();
    }

    [Fact]
    public async Task Handle_Should_ThrowDomainException_When_CategoryNotFound()
    {
        // Arrange
        const string categoryName = "Whatever";
        var currency = CurrencyPresets.USD;
        var user = UserPresets.JohnDoe;
        user.Currency = currency;
        var command = new CreateExpenseCommand(
            100M,
            categoryName,
            "Test comment",
            currency.Code,
            DateOnly.FromDateTime(DateTime.Now),
            user.Id);
        var validator = new CreateExpenseCommandValidator(
            ExpenseReadRepository,
            UserReadRepository);
        var handler = new CreateExpenseCommandHandler(
            CategoryReadRepository,
            CurrencyReadRepository,
            ExpenseWriteRepository,
            UserReadRepository,
            validator);

        CategoryReadRepository
            .ExistsAsync(Arg.Any<GetCategoryByNameSpecification>())
            .Returns(false);

        CurrencyReadRepository
            .ExistsAsync(Arg.Any<GetCurrencyByCodeSpecification>())
            .Returns(true);
        CurrencyReadRepository
            .GetAsync(Arg.Any<GetCurrencyByCodeSpecification>())
            .Returns([currency]);

        ExpenseReadRepository
            .ExistsAsync(Arg.Any<GetExpenseByUserIdAndAmountAndExecutionDate>())
            .Returns(false);

        UserReadRepository
            .ExistsAsync(Arg.Any<GetUserByIdSpecification>())
            .Returns(true);
        UserReadRepository
            .GetAsync(Arg.Any<GetUserByIdSpecification>())
            .Returns([user]);

        // Act
        var action = async () => await handler.Handle(command, CancellationToken.None);

        // Assert
        var actual = await action.ShouldThrowAsync<DomainException>();
        var expected = DomainException.CategoryNotFoundByName(categoryName);
        actual.Message.ShouldBe(expected.Message);
        actual.StatusCode.ShouldBe(expected.StatusCode);

        await ExpenseWriteRepository
            .Received(0)
            .AddAsync(Arg.Any<Expense>());
        await ExpenseWriteRepository
            .Received(0)
            .SaveChangesAsync();
    }

    [Fact]
    public async Task Handle_Should_ThrowDomainException_When_CurrencyNotFound()
    {
        // Arrange
        var category = CategoryPresets.Hotel;
        const string currencyCode = "JPY";
        var user = UserPresets.JohnDoe;

        var command = new CreateExpenseCommand(
            100M,
            category.Name,
            currencyCode,
            nameof(CreateExpenseCommand.Description),
            DateOnly.FromDateTime(DateTime.Now),
            user.Id);
        var validator = new CreateExpenseCommandValidator(
            ExpenseReadRepository,
            UserReadRepository);
        var handler = new CreateExpenseCommandHandler(
            CategoryReadRepository,
            CurrencyReadRepository,
            ExpenseWriteRepository,
            UserReadRepository,
            validator);

        CategoryReadRepository
            .ExistsAsync(Arg.Any<GetCategoryByNameSpecification>())
            .Returns(true);
        CategoryReadRepository
            .GetAsync(Arg.Any<GetCategoryByNameSpecification>())
            .Returns([category]);

        CurrencyReadRepository
            .ExistsAsync(Arg.Any<GetCurrencyByCodeSpecification>())
            .Returns(false);

        ExpenseReadRepository
            .ExistsAsync(Arg.Any<GetExpenseByUserIdAndAmountAndExecutionDate>())
            .Returns(false);

        UserReadRepository
            .ExistsAsync(Arg.Any<GetUserByIdSpecification>())
            .Returns(true);
        UserReadRepository
            .GetAsync(Arg.Any<GetUserByIdSpecification>())
            .Returns([user]);

        // Act
        var action = async () => await handler.Handle(command, CancellationToken.None);

        // Assert
        var actual = await action.ShouldThrowAsync<DomainException>();
        var expected = DomainException.CurrencyNotFoundByCode(currencyCode);
        actual.Message.ShouldBe(expected.Message);
        actual.StatusCode.ShouldBe(expected.StatusCode);

        await ExpenseWriteRepository
            .Received(0)
            .AddAsync(Arg.Any<Expense>());
        await ExpenseWriteRepository
            .Received(0)
            .SaveChangesAsync();
    }

    [Fact]
    public async Task Handle_Should_ThrowDomainException_When_UserNotFound()
    {
        // Arrange
        var category = CategoryPresets.Hotel;
        var currency = CurrencyPresets.USD;
        var userId = Guid.NewGuid();

        var command = new CreateExpenseCommand(
            100M,
            category.Name,
            "Test comment",
            currency.Code,
            DateOnly.FromDateTime(DateTime.Now),
            userId);
        var validator = new CreateExpenseCommandValidator(
            ExpenseReadRepository,
            UserReadRepository);
        var handler = new CreateExpenseCommandHandler(
            CategoryReadRepository,
            CurrencyReadRepository,
            ExpenseWriteRepository,
            UserReadRepository,
            validator);

        CategoryReadRepository
            .ExistsAsync(Arg.Any<GetCategoryByNameSpecification>())
            .Returns(true);
        CategoryReadRepository
            .GetAsync(Arg.Any<GetCategoryByNameSpecification>())
            .Returns([category]);

        CurrencyReadRepository
            .ExistsAsync(Arg.Any<GetCurrencyByCodeSpecification>())
            .Returns(true);
        CurrencyReadRepository
            .GetAsync(Arg.Any<GetCurrencyByCodeSpecification>())
            .Returns([currency]);

        ExpenseReadRepository
            .ExistsAsync(Arg.Any<GetExpenseByUserIdAndAmountAndExecutionDate>())
            .Returns(false);

        UserReadRepository
            .ExistsAsync(Arg.Any<GetUserByIdSpecification>())
            .Returns(false);

        // Act
        var action = async () => await handler.Handle(command, CancellationToken.None);

        // Assert
        var actual = await action.ShouldThrowAsync<DomainException>();
        var expected = DomainException.UserNotFoundById(userId);
        actual.Message.ShouldBe(expected.Message);
        actual.StatusCode.ShouldBe(expected.StatusCode);

        await ExpenseWriteRepository
            .Received(0)
            .AddAsync(Arg.Any<Expense>());
        await ExpenseWriteRepository
            .Received(0)
            .SaveChangesAsync();
    }
}
