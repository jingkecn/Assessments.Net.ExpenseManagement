using Assessments.ExpenseManagement.Application.Commands;
using Assessments.ExpenseManagement.Application.Specifications;
using Assessments.ExpenseManagement.Domain.Exceptions;
using Assessments.ExpenseManagement.Domain.Models;
using Assessments.ExpenseManagement.Infrastructure.Presets;
using Assessments.ExpenseManagement.UnitTests.Abstractions;
using NSubstitute;
using Shouldly;

namespace Assessments.ExpenseManagement.UnitTests.Application.Commands;

public sealed class DeleteExpenseCommandHandlerTests : TestBase
{
    [Fact]
    public async Task Handle_Should_ReturnTrue_When_Successful()
    {
        // Arrange
        var employee = EmployeePresets.JohnDoe;
        var expense = Expense.Create(
            employee,
            100M,
            CategoryPresets.Hotel,
            CurrencyPresets.CNY,
            DateOnly.FromDateTime(DateTime.Now),
            [Receipt.Create("No comment", "receipt.jpg")]);
        var command = new DeleteExpenseCommand(expense.Id);
        var handler = new DeleteExpenseCommandHandler(ExpenseReadRepository, ExpenseWriteRepository);

        ExpenseReadRepository
            .ExistsAsync(Arg.Any<GetExpenseByIdSpecification>())
            .Returns(true);
        ExpenseReadRepository
            .GetAsync(Arg.Any<GetExpenseByIdSpecification>())
            .Returns([expense]);

        ExpenseWriteRepository
            .DeleteAsync(expense)
            .Returns(true);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.ShouldBeTrue();

        await ExpenseReadRepository
            .Received(1)
            .ExistsAsync(Arg.Any<GetExpenseByIdSpecification>());
        await ExpenseReadRepository
            .Received(1)
            .GetAsync(Arg.Any<GetExpenseByIdSpecification>());

        await ExpenseWriteRepository
            .Received(1)
            .DeleteAsync(expense);
        await ExpenseWriteRepository.UnitOfWork
            .Received(1)
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_Should_ThrowDomainException_When_ExpenseNotFound()
    {
        // Arrange
        var expenseId = Guid.NewGuid();
        var command = new DeleteExpenseCommand(expenseId);
        var handler = new DeleteExpenseCommandHandler(ExpenseReadRepository, ExpenseWriteRepository);

        ExpenseReadRepository
            .ExistsAsync(Arg.Any<GetExpenseByIdSpecification>())
            .Returns(false);

        // Act
        var action = async () => await handler.Handle(command, CancellationToken.None);

        // Assert
        var exception = await action.ShouldThrowAsync<DomainException>();
        var expected = DomainException.ExpenseNotFoundById(command.Id);
        exception.Message.ShouldBe(expected.Message);
        exception.StatusCode.ShouldBe(expected.StatusCode);

        await ExpenseReadRepository
            .Received(1)
            .ExistsAsync(Arg.Any<GetExpenseByIdSpecification>());
        await ExpenseReadRepository
            .Received(0)
            .GetAsync(Arg.Any<GetExpenseByIdSpecification>());

        await ExpenseWriteRepository
            .Received(0)
            .DeleteAsync(Arg.Any<Expense>());
        await ExpenseWriteRepository.UnitOfWork
            .Received(0)
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
