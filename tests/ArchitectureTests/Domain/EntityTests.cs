using System.Reflection;
using Assessments.ExpenseManagement.ArchitectureTests.Abstractions;
using Assessments.ExpenseManagement.Domain.Abstractions;
using NetArchTest.Rules;
using Shouldly;

namespace Assessments.ExpenseManagement.ArchitectureTests.Domain;

public sealed class EntityTests : TestBase
{
    [Fact]
    public void Entity_Should_BeSealed()
    {
        // Arrange
        var entityTypes = Types
            .InAssembly(DomainAssembly)
            .That()
            .Inherit(typeof(Entity));

        // Act
        var result = entityTypes
            .Should()
            .BeSealed()
            .GetResult();

        // Assert
        result.IsSuccessful.ShouldBeTrue();
    }

    [Fact]
    public void Entity_Should_HavePrivateParameterlessConstructor()
    {
        // Arrange
        var entityTypes = Types
            .InAssembly(DomainAssembly)
            .That()
            .Inherit(typeof(Entity))
            .GetTypes();

        // Act
        var invalidEntityTypes = entityTypes
            .Select(type => new
            {
                type, constructors = type.GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance)
            })
            .Where(t => !t.constructors.Any(c => c.IsPrivate && c.GetParameters().Length is 0))
            .Select(t => t.type);

        // Assert
        invalidEntityTypes.ShouldBeEmpty();
    }

    [Fact]
    public void Entity_ShouldNot_HaveNameEndingWithEntity()
    {
        // Arrange
        var entityTypes = Types
            .InAssembly(DomainAssembly)
            .That()
            .Inherit(typeof(Entity));

        // Act
        var result = entityTypes
            .ShouldNot()
            .HaveNameEndingWith(nameof(Entity))
            .GetResult();

        // Assert
        result.IsSuccessful.ShouldBeTrue();
    }
}
