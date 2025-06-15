using Assessments.ExpenseManagement.ArchitectureTests.Abstractions;
using NetArchTest.Rules;
using Shouldly;

namespace Assessments.ExpenseManagement.ArchitectureTests.Layers;

public sealed class ApplicationTests : TestBase
{
    [Fact]
    public void Application_ShouldNot_HaveDependenciesOn_Infrastructure()
    {
        // Arrange
        // Act
        var result = Types
            .InAssembly(ApplicationAssembly)
            .ShouldNot()
            .HaveDependencyOn(InfrastructureNamespace)
            .GetResult();

        // Assert
        result.IsSuccessful.ShouldBeTrue();
    }

    [Fact]
    public void Application_ShouldNot_HaveDependenciesOn_CommandApi()
    {
        // Arrange
        // Act
        var result = Types
            .InAssembly(ApplicationAssembly)
            .ShouldNot()
            .HaveDependencyOn(CommandApiNamespace)
            .GetResult();

        // Assert
        result.IsSuccessful.ShouldBeTrue();
    }

    [Fact]
    public void Application_ShouldNot_HaveDependenciesOn_QueryApi()
    {
        // Arrange
        // Act
        var result = Types
            .InAssembly(ApplicationAssembly)
            .ShouldNot()
            .HaveDependencyOn(QueryApiNamespace)
            .GetResult();

        // Assert
        result.IsSuccessful.ShouldBeTrue();
    }
}
