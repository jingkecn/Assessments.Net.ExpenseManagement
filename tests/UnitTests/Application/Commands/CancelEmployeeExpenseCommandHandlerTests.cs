using Assessments.ExpenseManagement.Application.Commands;
using Assessments.ExpenseManagement.Application.Specifications;
using Assessments.ExpenseManagement.Domain.Exceptions;
using Assessments.ExpenseManagement.Domain.Models;
using Assessments.ExpenseManagement.Infrastructure.Presets;
using Assessments.ExpenseManagement.UnitTests.Abstractions;
using NSubstitute;
using Shouldly;

namespace Assessments.ExpenseManagement.UnitTests.Application.Commands;

public sealed class CancelEmployeeExpenseCommandHandlerTests : TestBase
{
    [Fact]
    public async Task Handle_Should_ReturnTrue_When_Successful()
    {
        // Arrange
        var employee = EmployeePresets.JaneDoe;
        var command = new CancelEmployeeExpenseCommand(employee.Id, Guid.NewGuid());
        var handler = new CancelEmployeeExpenseCommandHandler(ExpenseReadRepository, ExpenseWriteRepository);

        var expense = Expense.Create(
            employee,
            100M,
            CategoryPresets.Hotel,
            CurrencyPresets.CNY,
            DateOnly.FromDateTime(DateTime.Now),
            [Receipt.Create("No comment", "receipt.jpg")]);

        ExpenseReadRepository
            .ExistsAsync(Arg.Any<GetEmployeeExpenseByIdSpecification>())
            .Returns(true);
        ExpenseReadRepository
            .GetAsync(Arg.Any<GetEmployeeExpenseByIdSpecification>())
            .Returns([expense]);

        ExpenseWriteRepository
            .UpdateAsync(expense)
            .Returns(true);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.ShouldBeTrue();

        await ExpenseReadRepository
            .Received(1)
            .ExistsAsync(Arg.Any<GetEmployeeExpenseByIdSpecification>());
        await ExpenseReadRepository
            .Received(1)
            .GetAsync(Arg.Any<GetEmployeeExpenseByIdSpecification>());

        await ExpenseWriteRepository
            .Received(1)
            .UpdateAsync(expense);
        await ExpenseWriteRepository.UnitOfWork
            .Received(1)
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_Should_ThrowDomainException_When_ExpenseNotFound()
    {
        // Arrange
        var employee = EmployeePresets.JaneDoe;
        var command = new CancelEmployeeExpenseCommand(employee.Id, Guid.NewGuid());
        var handler = new CancelEmployeeExpenseCommandHandler(ExpenseReadRepository, ExpenseWriteRepository);

        ExpenseReadRepository
            .ExistsAsync(Arg.Any<GetEmployeeExpenseByIdSpecification>())
            .Returns(false);

        // Act
        var action = async () => await handler.Handle(command, CancellationToken.None);

        // Assert
        var exception = await action.ShouldThrowAsync<DomainException>();
        var expected = DomainException.ExpenseNotFoundByIdForEmployee(command.ExpenseId, command.EmployeeId);
        exception.Message.ShouldBe(expected.Message);
        exception.StatusCode.ShouldBe(expected.StatusCode);

        await ExpenseReadRepository
            .Received(1)
            .ExistsAsync(Arg.Any<GetEmployeeExpenseByIdSpecification>());
        await ExpenseReadRepository
            .Received(0)
            .GetAsync(Arg.Any<GetEmployeeExpenseByIdSpecification>());

        await ExpenseWriteRepository
            .Received(0)
            .UpdateAsync(Arg.Any<Expense>());
        await ExpenseWriteRepository.UnitOfWork
            .Received(0)
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
