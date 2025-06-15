using Assessments.ExpenseManagement.ArchitectureTests.Abstractions;
using NetArchTest.Rules;
using Shouldly;

namespace Assessments.ExpenseManagement.ArchitectureTests.Layers;

public sealed class DomainTests : TestBase
{
    [Fact]
    public void Domain_ShouldNot_HaveDependenciesOn_Application()
    {
        // Arrange
        // Act
        var result = Types
            .InAssembly(DomainAssembly)
            .ShouldNot()
            .HaveDependencyOn(ApplicationNamespace)
            .GetResult();

        // Assert
        result.IsSuccessful.ShouldBeTrue();
    }

    [Fact]
    public void Domain_ShouldNot_HaveDependenciesOn_Infrastructure()
    {
        // Arrange
        // Act
        var result = Types
            .InAssembly(DomainAssembly)
            .ShouldNot()
            .HaveDependencyOn(InfrastructureNamespace)
            .GetResult();

        // Assert
        result.IsSuccessful.ShouldBeTrue();
    }

    [Fact]
    public void Domain_ShouldNot_HaveDependenciesOn_CommandApi()
    {
        // Arrange
        // Act
        var result = Types
            .InAssembly(DomainAssembly)
            .ShouldNot()
            .HaveDependencyOn(CommandApiNamespace)
            .GetResult();

        // Assert
        result.IsSuccessful.ShouldBeTrue();
    }

    [Fact]
    public void Domain_ShouldNot_HaveDependenciesOn_QueryApi()
    {
        // Arrange
        // Act
        var result = Types
            .InAssembly(DomainAssembly)
            .ShouldNot()
            .HaveDependencyOn(QueryApiNamespace)
            .GetResult();

        // Assert
        result.IsSuccessful.ShouldBeTrue();
    }
}
