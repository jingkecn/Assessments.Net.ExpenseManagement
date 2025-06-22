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

public sealed class GetExpenseByIdQueryHandlerTests : TestBase
{
    [Fact]
    public async Task Handle_Should_GetExpense_WhenSuccess()
    {
        // Arrange
        var expected = Expense.Create(
            EmployeePresets.JohnDoe,
            100M,
            CategoryPresets.Restaurant,
            CurrencyPresets.USD,
            DateOnly.FromDateTime(DateTime.Now),
            [Receipt.Create("No comment", "john.doe/receipt.jpg")]);
        ;
        var query = new GetExpenseByIdQuery(expected.Id);
        var handler = new GetExpenseByIdQueryHandler(ExpenseReadRepository);

        ExpenseReadRepository
            .ExistsAsync(Arg.Any<GetExpenseByIdSpecification>())
            .Returns(true);
        ExpenseReadRepository
            .GetAsync(Arg.Any<GetExpenseByIdSpecification>(), Arg.Any<int>(), Arg.Any<int>())
            .Returns([expected]);

        // Act
        var actual = await handler.Handle(query, CancellationToken.None);

        // Assert
        actual.ShouldBeEquivalentTo(expected.ToView());

        await ExpenseReadRepository
            .Received(1)
            .ExistsAsync(Arg.Any<GetExpenseByIdSpecification>());
        await ExpenseReadRepository
            .Received(1)
            .GetAsync(Arg.Any<GetExpenseByIdSpecification>(), Arg.Any<int>(), Arg.Any<int>());
    }

    [Fact]
    public async Task Handle_Should_ThrowDomainException_WhenExpenseNotFound()
    {
        // Arrange
        var query = new GetExpenseByIdQuery(Guid.NewGuid());
        var handler = new GetExpenseByIdQueryHandler(ExpenseReadRepository);

        ExpenseReadRepository
            .ExistsAsync(Arg.Any<GetExpenseByIdSpecification>())
            .Returns(false);

        // Act
        var action = async () => await handler.Handle(query, CancellationToken.None);

        // Assert
        var exception = await action.ShouldThrowAsync<DomainException>();
        var expected = DomainException.ExpenseNotFoundById(query.Id);
        exception.Message.ShouldBe(expected.Message);
        exception.StatusCode.ShouldBe(expected.StatusCode);

        await ExpenseReadRepository
            .Received(1)
            .ExistsAsync(Arg.Any<GetExpenseByIdSpecification>());
        await ExpenseReadRepository
            .Received(0)
            .GetAsync(Arg.Any<GetExpenseByIdSpecification>(), Arg.Any<int>(), Arg.Any<int>());
    }
}
