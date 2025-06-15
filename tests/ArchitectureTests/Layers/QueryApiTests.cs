using Assessments.ExpenseManagement.ArchitectureTests.Abstractions;
using NetArchTest.Rules;
using Shouldly;

namespace Assessments.ExpenseManagement.ArchitectureTests.Layers;

public sealed class QueryApiTests : TestBase
{
    [Fact]
    public void QueryApi_ShouldNot_HaveDependenciesOn_CommandApi()
    {
        // Arrange
        // Act
        var result = Types
            .InAssembly(QueryApiAssembly)
            .ShouldNot()
            .HaveDependencyOn(CommandApiNamespace)
            .GetResult();

        // Assert
        result.IsSuccessful.ShouldBeTrue();
    }
}
