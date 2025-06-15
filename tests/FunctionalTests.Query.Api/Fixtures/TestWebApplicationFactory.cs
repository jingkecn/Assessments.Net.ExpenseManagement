using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;

namespace Assessments.ExpenseManagement.FunctionalTests.Query.Api.Fixtures;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public class TestWebApplicationFactory : WebApplicationFactory<ExpenseManagement.Query.Api.Program>, IAsyncLifetime
{
    private MsSqlContainer Container { get; } = new MsSqlBuilder()
        .WithPassword("$tr0ngP@$$w0rd")
        .Build();

    public IServiceProvider Provider { get; private set; } = null!;

    private IServiceScope Scope { get; set; } = null!;

    public async Task InitializeAsync()
    {
        await Container.StartAsync();
        Scope = Services.CreateScope();
        Provider = Scope.ServiceProvider;
    }

    public new async Task DisposeAsync()
    {
        Scope.Dispose();
        await Container.StopAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder) =>
        builder.UseSetting("ConnectionStrings:expense-management-db", Container.GetConnectionString());
}
