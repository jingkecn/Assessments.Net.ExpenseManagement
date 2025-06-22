using Assessments.ExpenseManagement.IntegrationTests.Fixtures;
using Microsoft.EntityFrameworkCore;

namespace Assessments.ExpenseManagement.IntegrationTests.Abstractions;

public abstract class TestBaseWithDbMigration(TestContainerFactory factory) : TestBase(factory)
{
    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
        await Context.Database.MigrateAsync();
    }
}
