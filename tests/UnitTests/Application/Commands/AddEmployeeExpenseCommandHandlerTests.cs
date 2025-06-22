using Assessments.ExpenseManagement.Application.Commands;
using Assessments.ExpenseManagement.Application.Models;
using Assessments.ExpenseManagement.Application.Specifications;
using Assessments.ExpenseManagement.Application.Validations;
using Assessments.ExpenseManagement.Domain.Exceptions;
using Assessments.ExpenseManagement.Domain.Models;
using Assessments.ExpenseManagement.Infrastructure.Presets;
using Assessments.ExpenseManagement.UnitTests.Abstractions;
using FluentValidation;
using NSubstitute;
using Shouldly;

namespace Assessments.ExpenseManagement.UnitTests.Application.Commands;

public sealed class AddEmployeeExpenseCommandHandlerTests : TestBase
{
    private IValidator<AddEmployeeExpenseCommand> Validator { get; } = new AddEmployeeExpenseCommandValidator();

    [Fact]
    public async Task Handle_Should_ReturnExpenseId_When_Successful()
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
            [new ReceiptView("No comment", "receipt.jpg")]);
        var handler = new AddEmployeeExpenseCommandHandler(
            CategoryReadRepository,
            CurrencyReadRepository,
            EmployeeReadRepository,
            ExpenseWriteRepository,
            ReceiptWriteRepository,
            Validator);

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

        EmployeeReadRepository
            .ExistsAsync(Arg.Any<GetEmployeeByIdSpecification>())
            .Returns(true);
        EmployeeReadRepository
            .GetAsync(Arg.Any<GetEmployeeByIdSpecification>())
            .Returns([employee]);

        ExpenseWriteRepository
            .AddAsync(Arg.Any<Expense>())
            .Returns(Expense.Create(
                employee,
                command.Amount,
                category,
                currency,
                DateOnly.FromDateTime(DateTime.Now)));

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.ShouldNotBe(Guid.Empty);

        await CategoryReadRepository
            .Received(1)
            .ExistsAsync(Arg.Any<GetCategoryByNameSpecification>());

        await CurrencyReadRepository
            .Received(1)
            .ExistsAsync(Arg.Any<GetCurrencyByCodeSpecification>());

        await ExpenseWriteRepository
            .Received(1)
            .AddAsync(Arg.Any<Expense>());
        await ExpenseWriteRepository.UnitOfWork
            .Received(1)
            .SaveChangesAsync(Arg.Any<CancellationToken>());

        await ReceiptWriteRepository
            .Received(1)
            .AddAsync(Arg.Any<Receipt>());
    }

    [Fact]
    public async Task Handle_Should_ThrowDomainException_When_CategoryNotFound()
    {
        // Arrange
        var currency = CurrencyPresets.CNY;
        var employee = EmployeePresets.JaneDoe;
        var command = new AddEmployeeExpenseCommand(
            100M,
            "Whatever",
            currency.Code,
            employee.Id,
            DateOnly.FromDateTime(DateTime.Now),
            [new ReceiptView("No comment", "receipt.jpg")]);
        var handler = new AddEmployeeExpenseCommandHandler(
            CategoryReadRepository,
            CurrencyReadRepository,
            EmployeeReadRepository,
            ExpenseWriteRepository,
            ReceiptWriteRepository,
            Validator);

        CategoryReadRepository
            .ExistsAsync(Arg.Any<GetCategoryByNameSpecification>())
            .Returns(false);

        CurrencyReadRepository
            .ExistsAsync(Arg.Any<GetCurrencyByCodeSpecification>())
            .Returns(true);
        CurrencyReadRepository
            .GetAsync(Arg.Any<GetCurrencyByCodeSpecification>())
            .Returns([currency]);

        EmployeeReadRepository
            .ExistsAsync(Arg.Any<GetEmployeeByIdSpecification>())
            .Returns(true);
        EmployeeReadRepository
            .GetAsync(Arg.Any<GetEmployeeByIdSpecification>())
            .Returns([employee]);

        // Act
        var action = async () => await handler.Handle(command, CancellationToken.None);

        // Assert
        var exception = await action.ShouldThrowAsync<DomainException>();
        var expected = DomainException.CategoryNotFoundByName(command.CategoryName);
        exception.Message.ShouldBe(expected.Message);
        exception.StatusCode.ShouldBe(expected.StatusCode);

        await CategoryReadRepository
            .Received(1)
            .ExistsAsync(Arg.Any<GetCategoryByNameSpecification>());

        await ExpenseWriteRepository
            .Received(0)
            .AddAsync(Arg.Any<Expense>());
        await ExpenseWriteRepository.UnitOfWork
            .Received(0)
            .SaveChangesAsync(Arg.Any<CancellationToken>());

        await ReceiptWriteRepository
            .Received(0)
            .AddAsync(Arg.Any<Receipt>());
        await ReceiptWriteRepository.UnitOfWork
            .Received(0)
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_Should_ThrowDomainException_When_CurrencyNotFound()
    {
        // Arrange
        var category = CategoryPresets.Hotel;
        var employee = EmployeePresets.JaneDoe;
        var command = new AddEmployeeExpenseCommand(
            100M,
            category.Name,
            "JPY",
            employee.Id,
            DateOnly.FromDateTime(DateTime.Now),
            [new ReceiptView("No comment", "receipt.jpg")]);
        var handler = new AddEmployeeExpenseCommandHandler(
            CategoryReadRepository,
            CurrencyReadRepository,
            EmployeeReadRepository,
            ExpenseWriteRepository,
            ReceiptWriteRepository,
            Validator);

        CategoryReadRepository
            .ExistsAsync(Arg.Any<GetCategoryByNameSpecification>())
            .Returns(true);
        CategoryReadRepository
            .GetAsync(Arg.Any<GetCategoryByNameSpecification>())
            .Returns([category]);

        CurrencyReadRepository
            .ExistsAsync(Arg.Any<GetCurrencyByCodeSpecification>())
            .Returns(false);

        EmployeeReadRepository
            .ExistsAsync(Arg.Any<GetEmployeeByIdSpecification>())
            .Returns(true);
        EmployeeReadRepository
            .GetAsync(Arg.Any<GetEmployeeByIdSpecification>())
            .Returns([employee]);

        // Act
        var action = async () => await handler.Handle(command, CancellationToken.None);

        // Assert
        var exception = await action.ShouldThrowAsync<DomainException>();
        var expected = DomainException.CurrencyNotFoundByCode(command.CurrencyCode);
        exception.Message.ShouldBe(expected.Message);
        exception.StatusCode.ShouldBe(expected.StatusCode);

        await CurrencyReadRepository
            .Received(1)
            .ExistsAsync(Arg.Any<GetCurrencyByCodeSpecification>());

        await ExpenseWriteRepository
            .Received(0)
            .AddAsync(Arg.Any<Expense>());
        await ExpenseWriteRepository.UnitOfWork
            .Received(0)
            .SaveChangesAsync(Arg.Any<CancellationToken>());

        await ReceiptWriteRepository
            .Received(0)
            .AddAsync(Arg.Any<Receipt>());
        await ReceiptWriteRepository.UnitOfWork
            .Received(0)
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_Should_ThrowDomainException_When_EmployeeNotFound()
    {
        // Arrange
        var category = CategoryPresets.Hotel;
        var currency = CurrencyPresets.CNY;
        var command = new AddEmployeeExpenseCommand(
            100M,
            category.Name,
            currency.Code,
            Guid.NewGuid(),
            DateOnly.FromDateTime(DateTime.Now),
            [new ReceiptView("No comment", "receipt.jpg")]);
        var handler = new AddEmployeeExpenseCommandHandler(
            CategoryReadRepository,
            CurrencyReadRepository,
            EmployeeReadRepository,
            ExpenseWriteRepository,
            ReceiptWriteRepository,
            Validator);

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

        EmployeeReadRepository
            .ExistsAsync(Arg.Any<GetEmployeeByIdSpecification>())
            .Returns(false);

        // Act
        var action = async () => await handler.Handle(command, CancellationToken.None);

        // Assert
        var exception = await action.ShouldThrowAsync<DomainException>();
        var expected = DomainException.EmployeeNotFoundById(command.EmployeeId);
        exception.Message.ShouldBe(expected.Message);
        exception.StatusCode.ShouldBe(expected.StatusCode);

        await CurrencyReadRepository
            .Received(1)
            .ExistsAsync(Arg.Any<GetCurrencyByCodeSpecification>());

        await ExpenseWriteRepository
            .Received(0)
            .AddAsync(Arg.Any<Expense>());
        await ExpenseWriteRepository.UnitOfWork
            .Received(0)
            .SaveChangesAsync(Arg.Any<CancellationToken>());

        await ReceiptWriteRepository
            .Received(0)
            .AddAsync(Arg.Any<Receipt>());
        await ReceiptWriteRepository.UnitOfWork
            .Received(0)
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
