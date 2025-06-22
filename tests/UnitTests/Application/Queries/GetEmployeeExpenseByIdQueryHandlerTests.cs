using Assessments.ExpenseManagement.Application.Extensions;
using Assessments.ExpenseManagement.Application.Queries;
using Assessments.ExpenseManagement.Application.Specifications;
using Assessments.ExpenseManagement.Domain.Exceptions;
using Assessments.ExpenseManagement.Domain.Models;
using Assessments.ExpenseManagement.Infrastructure.Presets;
using Assessments.ExpenseManagement.UnitTests.Abstractions;
using NSubstitute;
using Shouldly;

namespace Assessments.ExpenseManagement.UnitTests.Application.Queries;

public sealed class GetEmployeeExpenseByIdQueryHandlerTests : TestBase
{
    [Fact]
    public async Task Handle_Should_GetExpense_WhenSuccess()
    {
        // Arrange
        var employee = EmployeePresets.JohnDoe;
        var expense = Expense.Create(
            employee,
            100M,
            CategoryPresets.Restaurant,
            CurrencyPresets.USD,
            DateOnly.FromDateTime(DateTime.Now),
            [Receipt.Create("No comment", "john.doe/receipt.jpg")]);
        var query = new GetEmployeeExpenseByIdQuery(employee.Id, expense.Id);
        var handler = new GetEmployeeExpenseByIdQueryHandler(EmployeeReadRepository, ExpenseReadRepository);

        EmployeeReadRepository
            .ExistsAsync(Arg.Any<GetEmployeeByIdSpecification>())
            .Returns(true);
        EmployeeReadRepository
            .GetAsync(Arg.Any<GetEmployeeByIdSpecification>(), Arg.Any<int>(), Arg.Any<int>())
            .Returns([employee]);

        ExpenseReadRepository
            .ExistsAsync(Arg.Any<GetEmployeeExpenseByIdSpecification>())
            .Returns(true);
        ExpenseReadRepository
            .GetAsync(Arg.Any<GetEmployeeExpenseByIdSpecification>(), Arg.Any<int>(), Arg.Any<int>())
            .Returns([expense]);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.ShouldBeEquivalentTo(expense.ToView());

        await EmployeeReadRepository
            .Received(1)
            .ExistsAsync(Arg.Any<GetEmployeeByIdSpecification>());

        await ExpenseReadRepository
            .Received(1)
            .ExistsAsync(Arg.Any<GetEmployeeExpenseByIdSpecification>());
        await ExpenseReadRepository
            .Received(1)
            .GetAsync(Arg.Any<GetEmployeeExpenseByIdSpecification>(), Arg.Any<int>(), Arg.Any<int>());
    }

    [Fact]
    public async Task Handle_Should_ThrowDomainException_WhenEmployeeNotFound()
    {
        // Arrange
        var query = new GetEmployeeExpenseByIdQuery(Guid.NewGuid(), Guid.NewGuid());
        var handler = new GetEmployeeExpenseByIdQueryHandler(EmployeeReadRepository, ExpenseReadRepository);

        EmployeeReadRepository
            .ExistsAsync(Arg.Any<GetEmployeeByIdSpecification>())
            .Returns(false);

        // Act
        var action = async () => await handler.Handle(query, CancellationToken.None);

        // Assert
        var exception = await action.ShouldThrowAsync<DomainException>();
        var expected = DomainException.EmployeeNotFoundById(query.EmployeeId);
        exception.Message.ShouldBe(expected.Message);
        exception.StatusCode.ShouldBe(expected.StatusCode);

        await EmployeeReadRepository
            .Received(1)
            .ExistsAsync(Arg.Any<GetEmployeeByIdSpecification>());

        await ExpenseReadRepository
            .Received(0)
            .ExistsAsync(Arg.Any<GetEmployeeExpenseByIdSpecification>());
        await ExpenseReadRepository
            .Received(0)
            .GetAsync(Arg.Any<GetEmployeeExpenseByIdSpecification>(), Arg.Any<int>(), Arg.Any<int>());
    }

    [Fact]
    public async Task Handle_Should_ThrowDomainException_WhenExpenseNotFound()
    {
        // Arrange
        var employee = EmployeePresets.JohnDoe;
        var query = new GetEmployeeExpenseByIdQuery(employee.Id, Guid.NewGuid());
        var handler = new GetEmployeeExpenseByIdQueryHandler(EmployeeReadRepository, ExpenseReadRepository);

        EmployeeReadRepository
            .ExistsAsync(Arg.Any<GetEmployeeByIdSpecification>())
            .Returns(true);
        EmployeeReadRepository
            .GetAsync(Arg.Any<GetEmployeeByIdSpecification>(), Arg.Any<int>(), Arg.Any<int>())
            .Returns([employee]);

        ExpenseReadRepository
            .ExistsAsync(Arg.Any<GetExpenseByIdSpecification>())
            .Returns(false);

        // Act
        var action = async () => await handler.Handle(query, CancellationToken.None);

        // Assert
        var exception = await action.ShouldThrowAsync<DomainException>();
        var expected = DomainException.ExpenseNotFoundByIdForEmployee(query.ExpenseId, query.EmployeeId);
        exception.Message.ShouldBe(expected.Message);
        exception.StatusCode.ShouldBe(expected.StatusCode);

        await EmployeeReadRepository
            .Received(1)
            .ExistsAsync(Arg.Any<GetEmployeeByIdSpecification>());

        await ExpenseReadRepository
            .Received(1)
            .ExistsAsync(Arg.Any<GetEmployeeExpenseByIdSpecification>());
        await ExpenseReadRepository
            .Received(0)
            .GetAsync(Arg.Any<GetEmployeeExpenseByIdSpecification>(), Arg.Any<int>(), Arg.Any<int>());
    }
}
