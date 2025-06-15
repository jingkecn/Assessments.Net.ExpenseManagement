using System.Diagnostics.CodeAnalysis;
using Testcontainers.MsSql;

namespace Assessments.ExpenseManagement.IntegrationTests.Fixtures;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public class TestContainerFactory : IAsyncLifetime
{
    private MsSqlContainer Container { get; } = new MsSqlBuilder()
        .WithPassword("$tr0ngP@$$w0rd")
        .Build();

    public string ConnectionString => Container.GetConnectionString();

    public async Task InitializeAsync() => await Container.StartAsync();

    public async Task DisposeAsync() => await Container.StopAsync();
}
