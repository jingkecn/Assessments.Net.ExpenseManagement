using Assessments.ExpenseManagement.Application.Commands;
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

public sealed class AddEmployeeCommandHandlerTests : TestBase
{
    private IValidator<AddEmployeeCommand> Validator { get; } = new AddEmployeeCommandValidator();

    [Fact]
    public async Task Handle_Should_ReturnEmployeeId_When_Successful()
    {
        // Arrange
        var command = new AddEmployeeCommand("Jane", "Doe", "CNY");
        var handler = new AddEmployeeCommandHandler(CurrencyReadRepository, EmployeeWriteRepository, Validator);

        CurrencyReadRepository
            .ExistsAsync(Arg.Any<GetCurrencyByCodeSpecification>())
            .Returns(true);
        CurrencyReadRepository
            .GetAsync(Arg.Any<GetCurrencyByCodeSpecification>())
            .Returns([CurrencyPresets.CNY]);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.ShouldNotBe(Guid.Empty);

        await CurrencyReadRepository
            .Received(1)
            .ExistsAsync(Arg.Any<GetCurrencyByCodeSpecification>());

        await EmployeeWriteRepository
            .Received(1)
            .AddAsync(Arg.Is<Employee>(e => e.FirstName == command.FirstName && e.LastName == command.LastName));
        await EmployeeWriteRepository.UnitOfWork
            .Received(1)
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_Should_ThrowDomainException_When_CurrencyNotFound()
    {
        // Arrange
        var command = new AddEmployeeCommand("Jane", "Doe", "CNY");
        var handler = new AddEmployeeCommandHandler(CurrencyReadRepository, EmployeeWriteRepository, Validator);

        CurrencyReadRepository
            .ExistsAsync(Arg.Any<GetCurrencyByCodeSpecification>())
            .Returns(false);

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

        await EmployeeWriteRepository
            .Received(0)
            .AddAsync(Arg.Any<Employee>());
        await EmployeeWriteRepository.UnitOfWork
            .Received(0)
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
