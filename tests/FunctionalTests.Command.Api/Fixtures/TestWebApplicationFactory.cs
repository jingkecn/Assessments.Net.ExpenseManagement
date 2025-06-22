using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Testcontainers.MsSql;

namespace Assessments.ExpenseManagement.FunctionalTests.Command.Api.Fixtures;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public class TestWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private MsSqlContainer Container { get; } = new MsSqlBuilder()
        .WithPassword("$tr0ngP@$$w0rd")
        .Build();

    public async Task InitializeAsync() => await Container.StartAsync();

    public new async Task DisposeAsync() => await Container.StopAsync();

    protected override void ConfigureWebHost(IWebHostBuilder builder) =>
        builder.UseSetting("ConnectionStrings:expense-management-db", Container.GetConnectionString());
}
