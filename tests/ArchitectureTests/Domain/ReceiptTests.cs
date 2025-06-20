using Assessments.ExpenseManagement.ArchitectureTests.Abstractions;
using Assessments.ExpenseManagement.Domain.Abstractions;
using Assessments.ExpenseManagement.Domain.Contracts;
using Assessments.ExpenseManagement.Domain.Models;
using NetArchTest.Rules;
using Shouldly;

namespace Assessments.ExpenseManagement.ArchitectureTests.Domain;

public sealed class ReceiptTests : TestBase
{
    [Fact]
    public void Receipt_Should_InheritFromEntity()
    {
        // Arrange
        var entityTypes = Types
            .InAssembly(DomainAssembly)
            .That()
            .Inherit(typeof(Entity))
            .GetTypes();

        // Act
        var result = entityTypes.Contains(typeof(Receipt));

        // Assert
        result.ShouldBeTrue();
    }

    [Fact]
    public void Receipt_ShouldNot_BeAnAggregateRoot()
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
        var result = aggregateRootTypes.Contains(typeof(Receipt));

        // Assert
        result.ShouldBeFalse();
    }
}
