using Assessments.ExpenseManagement.ArchitectureTests.Abstractions;
using Assessments.ExpenseManagement.Domain.Abstractions;
using Assessments.ExpenseManagement.Domain.Contracts;
using Assessments.ExpenseManagement.Domain.Models;
using NetArchTest.Rules;
using Shouldly;

namespace Assessments.ExpenseManagement.ArchitectureTests.Domain;

public sealed class EmployeeTests : TestBase
{
    [Fact]
    public void Employee_Should_BeAnAggregateRoot()
    {
        // Arrange
        var aggregateRootTypes = Types
            .InAssembly(DomainAssembly)
            .That()
            .AreClasses()
            .And()
            .ImplementInterface(typeof(IAggregateRoot))
            .GetTypes();

        // Act
        var result = aggregateRootTypes.Contains(typeof(Employee));

        // Assert
        result.ShouldBeTrue();
    }

    [Fact]
    public void Employee_Should_InheritFromEntity()
    {
        // Arrange
        var entityTypes = Types
            .InAssembly(DomainAssembly)
            .That()
            .Inherit(typeof(Entity))
            .GetTypes();

        // Act
        var result = entityTypes.Contains(typeof(Employee));

        // Assert
        result.ShouldBeTrue();
    }
}
