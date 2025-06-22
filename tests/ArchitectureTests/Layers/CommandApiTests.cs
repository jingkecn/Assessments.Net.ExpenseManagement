using Assessments.ExpenseManagement.ArchitectureTests.Abstractions;
using NetArchTest.Rules;
using Shouldly;

namespace Assessments.ExpenseManagement.ArchitectureTests.Layers;

public sealed class CommandApiTests : TestBase
{
    [Fact]
    public void CommandApi_ShouldNot_HaveDependenciesOn_QueryApi()
    {
        // Arrange
        // Act
        var result = Types
            .InAssembly(CommandApiAssembly)
            .ShouldNot()
            .HaveDependencyOn(QueryApiNamespace)
            .GetResult();

        // Assert
        result.IsSuccessful.ShouldBeTrue();
    }
}
