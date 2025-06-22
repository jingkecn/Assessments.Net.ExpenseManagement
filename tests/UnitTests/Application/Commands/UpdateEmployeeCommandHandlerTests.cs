using Assessments.ExpenseManagement.Application.Commands;
using Assessments.ExpenseManagement.Application.Specifications;
using Assessments.ExpenseManagement.Application.Validations;
using Assessments.ExpenseManagement.Domain.Contracts;
using Assessments.ExpenseManagement.Domain.Exceptions;
using Assessments.ExpenseManagement.Domain.Models;
using Assessments.ExpenseManagement.Infrastructure.Presets;
using Assessments.ExpenseManagement.UnitTests.Abstractions;
using FluentValidation;
using NSubstitute;
using Shouldly;

namespace Assessments.ExpenseManagement.UnitTests.Application.Commands;

public sealed class UpdateEmployeeCommandHandlerTests : TestBase
{
    private IValidator<UpdateEmployeeCommand> Validator { get; } = new UpdateEmployeeCommandValidator();

    [Fact]
    public async Task Handle_Should_ReturnTrue_When_Successful()
    {
        // Arrange
        var currency = CurrencyPresets.CNY;
        var employee = EmployeePresets.JohnDoe;
        var command = new UpdateEmployeeCommand(employee.Id, currency.Code);
        var handler = new UpdateEmployeeCommandHandler(
            CurrencyReadRepository,
            EmployeeReadRepository,
            EmployeeWriteRepository,
            Validator);

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

        EmployeeWriteRepository
            .UpdateAsync(Arg.Any<Employee>())
            .Returns(true);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.ShouldBeTrue();

        await CurrencyReadRepository
            .Received(1)
            .ExistsAsync(Arg.Any<GetCurrencyByCodeSpecification>());
        await CurrencyReadRepository
            .Received(1)
            .GetAsync(Arg.Any<GetCurrencyByCodeSpecification>());

        await EmployeeReadRepository
            .Received(1)
            .ExistsAsync(Arg.Any<GetEmployeeByIdSpecification>());
        await EmployeeReadRepository
            .Received(1)
            .GetAsync(Arg.Any<GetEmployeeByIdSpecification>());

        await EmployeeWriteRepository
            .Received(1)
            .UpdateAsync(Arg.Any<Employee>());
        await EmployeeWriteRepository.UnitOfWork
            .Received(1)
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_Should_ThrowDomainException_When_CurrencyNotFound()
    {
        // Arrange
        const string currencyCode = "JPY";
        var employee = EmployeePresets.JohnDoe;
        var command = new UpdateEmployeeCommand(employee.Id, currencyCode);
        var handler = new UpdateEmployeeCommandHandler(
            CurrencyReadRepository,
            EmployeeReadRepository,
            EmployeeWriteRepository,
            Validator);

        CurrencyReadRepository
            .ExistsAsync(Arg.Any<GetCurrencyByCodeSpecification>())
            .Returns(false);

        EmployeeReadRepository
            .ExistsAsync(Arg.Any<GetEmployeeByIdSpecification>())
            .Returns(true);
        EmployeeReadRepository
            .GetAsync(Arg.Any<GetEmployeeByIdSpecification>())
            .Returns([employee]);

        EmployeeWriteRepository
            .UpdateAsync(Arg.Any<Employee>())
            .Returns(true);

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
        await CurrencyReadRepository
            .Received(0)
            .GetAsync(Arg.Any<GetCurrencyByCodeSpecification>());

        await EmployeeWriteRepository
            .Received(0)
            .UpdateAsync(Arg.Any<Employee>());
        await EmployeeWriteRepository.UnitOfWork
            .Received(0)
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_Should_ThrowDomainException_When_EmployeeDoesNotExist()
    {
        // Arrange
        var currency = CurrencyPresets.CNY;
        var employeeId = Guid.NewGuid();
        var command = new UpdateEmployeeCommand(employeeId, currency.Code);
        var handler = new UpdateEmployeeCommandHandler(
            CurrencyReadRepository,
            EmployeeReadRepository,
            EmployeeWriteRepository,
            Validator);

        CurrencyReadRepository
            .ExistsAsync(Arg.Any<ISpecification<Currency>>())
            .Returns(true);
        CurrencyReadRepository
            .GetAsync(Arg.Any<ISpecification<Currency>>())
            .Returns([currency]);

        EmployeeReadRepository
            .ExistsAsync(Arg.Any<ISpecification<Employee>>())
            .Returns(false);

        EmployeeWriteRepository
            .UpdateAsync(Arg.Any<Employee>())
            .Returns(true);

        // Act
        var action = async () => await handler.Handle(command, CancellationToken.None);

        // Assert
        var exception = await action.ShouldThrowAsync<DomainException>();
        var expected = DomainException.EmployeeNotFoundById(command.EmployeeId);
        exception.Message.ShouldBe(expected.Message);
        exception.StatusCode.ShouldBe(expected.StatusCode);

        await EmployeeReadRepository
            .Received(1)
            .ExistsAsync(Arg.Any<GetEmployeeByIdSpecification>());
        await EmployeeReadRepository
            .Received(0)
            .GetAsync(Arg.Any<GetEmployeeByIdSpecification>());

        await EmployeeWriteRepository
            .Received(0)
            .UpdateAsync(Arg.Any<Employee>());
        await EmployeeWriteRepository.UnitOfWork
            .Received(0)
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
