using Assessments.ExpenseManagement.IntegrationTests.Fixtures;

namespace Assessments.ExpenseManagement.IntegrationTests.Abstractions;

public abstract class TestBaseWithMigration(TestContainerFactory factory) : TestBase(factory)
{
    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
        await Context.Database.EnsureCreatedAsync();
    }
}
