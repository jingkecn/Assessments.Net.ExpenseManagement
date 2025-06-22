using Assessments.ExpenseManagement.Application.Commands;
using Assessments.ExpenseManagement.Application.Specifications;
using Assessments.ExpenseManagement.Domain.Exceptions;
using Assessments.ExpenseManagement.Domain.Models;
using Assessments.ExpenseManagement.Infrastructure.Presets;
using Assessments.ExpenseManagement.UnitTests.Abstractions;
using NSubstitute;
using Shouldly;

namespace Assessments.ExpenseManagement.UnitTests.Application.Commands;

public sealed class DeleteEmployeeCommandHandlerTests : TestBase
{
    [Fact]
    public async Task Handle_Should_ReturnTrue_When_Successful()
    {
        // Arrange
        var employee = EmployeePresets.JohnDoe;
        var command = new DeleteEmployeeCommand(employee.Id);
        var handler = new DeleteEmployeeCommandHandler(EmployeeReadRepository, EmployeeWriteRepository);

        EmployeeReadRepository
            .ExistsAsync(Arg.Any<GetEmployeeByIdSpecification>())
            .Returns(true);
        EmployeeReadRepository
            .GetAsync(Arg.Any<GetEmployeeByIdSpecification>())
            .Returns([employee]);

        EmployeeWriteRepository
            .DeleteAsync(employee)
            .Returns(true);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.ShouldBeTrue();

        await EmployeeReadRepository
            .Received(1)
            .ExistsAsync(Arg.Any<GetEmployeeByIdSpecification>());
        await EmployeeReadRepository
            .Received(1)
            .GetAsync(Arg.Any<GetEmployeeByIdSpecification>());

        await EmployeeWriteRepository
            .Received(1)
            .DeleteAsync(employee);
        await EmployeeWriteRepository.UnitOfWork
            .Received(1)
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_Should_ThrowDomainException_When_EmployeeNotFound()
    {
        // Arrange
        var employeeId = Guid.NewGuid();
        var command = new DeleteEmployeeCommand(employeeId);
        var handler = new DeleteEmployeeCommandHandler(EmployeeReadRepository, EmployeeWriteRepository);

        EmployeeReadRepository
            .ExistsAsync(Arg.Any<GetEmployeeByIdSpecification>())
            .Returns(false);

        // Act
        var action = async () => await handler.Handle(command, CancellationToken.None);

        // Assert
        var exception = await action.ShouldThrowAsync<DomainException>();
        var expected = DomainException.EmployeeNotFoundById(command.Id);
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
            .DeleteAsync(Arg.Any<Employee>());
        await EmployeeWriteRepository.UnitOfWork
            .Received(0)
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
