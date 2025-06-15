using Assessments.ExpenseManagement.ArchitectureTests.Abstractions;
using NetArchTest.Rules;
using Shouldly;

namespace Assessments.ExpenseManagement.ArchitectureTests.Layers;

public sealed class InfrastructureTests : TestBase
{
    [Fact]
    public void Infrastructure_ShouldNot_HaveDependenciesOn_Application()
    {
        // Arrange
        // Act
        var result = Types
            .InAssembly(InfrastructureAssembly)
            .ShouldNot()
            .HaveDependencyOn(ApplicationNamespace)
            .GetResult();

        // Assert
        result.IsSuccessful.ShouldBeTrue();
    }

    [Fact]
    public void Infrastructure_ShouldNot_HaveDependenciesOn_CommandApi()
    {
        // Arrange
        // Act
        var result = Types
            .InAssembly(InfrastructureAssembly)
            .ShouldNot()
            .HaveDependencyOn(CommandApiNamespace)
            .GetResult();

        // Assert
        result.IsSuccessful.ShouldBeTrue();
    }

    [Fact]
    public void Infrastructure_ShouldNot_HaveDependenciesOn_QueryApi()
    {
        // Arrange
        // Act
        var result = Types
            .InAssembly(InfrastructureAssembly)
            .ShouldNot()
            .HaveDependencyOn(QueryApiNamespace)
            .GetResult();

        // Assert
        result.IsSuccessful.ShouldBeTrue();
    }
}
