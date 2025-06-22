using Assessments.ExpenseManagement.Application.Commands;
using Assessments.ExpenseManagement.Application.Validations;
using Assessments.ExpenseManagement.Domain.Models;
using Assessments.ExpenseManagement.UnitTests.Abstractions;
using FluentValidation;
using Mediator;
using NSubstitute;
using Shouldly;

namespace Assessments.ExpenseManagement.UnitTests.Application.Commands;

public sealed class PatchEmployeeExpenseCommandHandlerTests : TestBase
{
    private IMediator Mediator { get; } = Substitute.For<IMediator>();
    private IValidator<PatchEmployeeExpenseCommand> Validator { get; } = new PatchEmployeeExpenseCommandValidator();

    [Fact]
    public async Task Handle_Should_ReturnTrue_When_SuccessfulWithCancelAction()
    {
        // Arrange
        var (employeeId, expenseId, action) = (Guid.NewGuid(), Guid.NewGuid(), nameof(Expense.Cancel));
        var command = new PatchEmployeeExpenseCommand(action, employeeId, expenseId);
        var handler = new PatchEmployeeExpenseCommandHandler(Mediator, Validator);

        Mediator
            .Send(Arg.Any<CancelEmployeeExpenseCommand>())
            .Returns(true);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.ShouldBeTrue();

        await Mediator
            .Received(1)
            .Send(Arg.Any<CancelEmployeeExpenseCommand>());
    }

    [Fact]
    public async Task Handle_Should_ReturnTrue_When_SuccessfulWithSubmitAction()
    {
        // Arrange
        var (employeeId, expenseId, action) = (Guid.NewGuid(), Guid.NewGuid(), nameof(Expense.Submit));
        var command = new PatchEmployeeExpenseCommand(action, employeeId, expenseId);
        var handler = new PatchEmployeeExpenseCommandHandler(Mediator, Validator);

        Mediator
            .Send(Arg.Any<SubmitEmployeeExpenseCommand>())
            .Returns(true);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.ShouldBeTrue();

        await Mediator
            .Received(1)
            .Send(Arg.Any<SubmitEmployeeExpenseCommand>());
    }
}
