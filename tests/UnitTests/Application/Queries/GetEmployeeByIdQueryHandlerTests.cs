using Assessments.ExpenseManagement.Application.Extensions;
using Assessments.ExpenseManagement.Application.Queries;
using Assessments.ExpenseManagement.Application.Specifications;
using Assessments.ExpenseManagement.Domain.Exceptions;
using Assessments.ExpenseManagement.Infrastructure.Presets;
using Assessments.ExpenseManagement.UnitTests.Abstractions;
using NSubstitute;
using Shouldly;

namespace Assessments.ExpenseManagement.UnitTests.Application.Queries;

public sealed class GetEmployeeByIdQueryHandlerTests : TestBase
{
    [Fact]
    public async Task Handle_Should_GetEmployee_WhenSuccess()
    {
        // Arrange
        var expected = EmployeePresets.JohnDoe;
        var query = new GetEmployeeByIdQuery(expected.Id);
        var handler = new GetEmployeeByIdQueryHandler(EmployeeReadRepository);

        EmployeeReadRepository
            .ExistsAsync(Arg.Any<GetEmployeeByIdSpecification>())
            .Returns(true);
        EmployeeReadRepository
            .GetAsync(Arg.Any<GetEmployeeByIdSpecification>(), Arg.Any<int>(), Arg.Any<int>())
            .Returns([expected]);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.ShouldBeEquivalentTo(expected.ToView());

        await EmployeeReadRepository
            .Received(1)
            .ExistsAsync(Arg.Any<GetEmployeeByIdSpecification>());
        await EmployeeReadRepository
            .Received(1)
            .GetAsync(Arg.Any<GetEmployeeByIdSpecification>(), Arg.Any<int>(), Arg.Any<int>());
    }

    [Fact]
    public async Task Handle_Should_ThrowDomainException_WhenEmployeeNotFound()
    {
        // Arrange
        var query = new GetEmployeeByIdQuery(Guid.NewGuid());
        var handler = new GetEmployeeByIdQueryHandler(EmployeeReadRepository);

        EmployeeReadRepository
            .ExistsAsync(Arg.Any<GetEmployeeByIdSpecification>())
            .Returns(false);

        // Act
        var action = async () => await handler.Handle(query, CancellationToken.None);

        // Assert
        var exception = await action.ShouldThrowAsync<DomainException>();
        var expected = DomainException.EmployeeNotFoundById(query.Id);
        exception.Message.ShouldBe(expected.Message);
        exception.StatusCode.ShouldBe(expected.StatusCode);

        await EmployeeReadRepository
            .Received(1)
            .ExistsAsync(Arg.Any<GetEmployeeByIdSpecification>());
        await EmployeeReadRepository
            .Received(0)
            .GetAsync(Arg.Any<GetEmployeeByIdSpecification>(), Arg.Any<int>(), Arg.Any<int>());
    }
}
