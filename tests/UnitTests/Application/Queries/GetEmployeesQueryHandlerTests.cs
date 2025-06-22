using Assessments.ExpenseManagement.Application.Extensions;
using Assessments.ExpenseManagement.Application.Queries;
using Assessments.ExpenseManagement.Application.Specifications;
using Assessments.ExpenseManagement.Application.Validations;
using Assessments.ExpenseManagement.Infrastructure.Presets;
using Assessments.ExpenseManagement.UnitTests.Abstractions;
using FluentValidation;
using NSubstitute;
using Shouldly;

namespace Assessments.ExpenseManagement.UnitTests.Application.Queries;

public sealed class GetEmployeesQueryHandlerTests : TestBase
{
    private IValidator<GetEmployeesQuery> Validator { get; } = new GetEmployeesQueryValidator();

    [Fact]
    public async Task Handle_Should_GetPaginatedEmployees_When_Successful()
    {
        // Arrange
        var query = new GetEmployeesQuery(1, 1);
        var handler = new GetEmployeesQueryHandler(EmployeeReadRepository, Validator);

        var expected = EmployeePresets.PredefinedEmployees.ToList();
        EmployeeReadRepository
            .CountAsync(Arg.Any<GetEmployeesSpecification>())
            .Returns(expected.Count);

        var expectedWithPagination = expected
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToList();
        EmployeeReadRepository
            .GetAsync(Arg.Any<GetEmployeesSpecification>(), query.PageNumber, query.PageSize)
            .Returns(expectedWithPagination);

        // Act
        var (employees, totalCount) = await handler.Handle(query, CancellationToken.None);

        // Assert
        employees.ShouldBe(expectedWithPagination.ToView(), true);
        totalCount.ShouldBe(expected.Count);

        await EmployeeReadRepository
            .Received(1)
            .CountAsync(Arg.Any<GetEmployeesSpecification>());
        await EmployeeReadRepository
            .Received(1)
            .GetAsync(Arg.Any<GetEmployeesSpecification>(), query.PageNumber, query.PageSize);
    }
}
