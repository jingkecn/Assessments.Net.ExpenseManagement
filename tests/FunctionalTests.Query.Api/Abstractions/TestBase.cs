using Assessments.ExpenseManagement.FunctionalTests.Query.Api.Fixtures;
using Assessments.ExpenseManagement.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Assessments.ExpenseManagement.FunctionalTests.Query.Api.Abstractions;

public abstract class TestBase(TestWebApplicationFactory factory) : IClassFixture<TestWebApplicationFactory>
{
    protected ExpenseDbContext Context { get; } =
        factory.Provider.GetRequiredService<ExpenseDbContext>();

    protected HttpClient HttpClient { get; } = factory.CreateClient();
}
