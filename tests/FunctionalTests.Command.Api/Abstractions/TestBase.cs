using Assessments.ExpenseManagement.FunctionalTests.Command.Api.Fixtures;

namespace Assessments.ExpenseManagement.FunctionalTests.Command.Api.Abstractions;

public abstract class TestBase(TestWebApplicationFactory factory) : IClassFixture<TestWebApplicationFactory>
{
    protected HttpClient HttpClient { get; } = factory.CreateClient();
}
