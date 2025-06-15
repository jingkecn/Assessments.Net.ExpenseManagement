using System.Data.Common;
using Assessments.ExpenseManagement.Domain.Contracts;
using Assessments.ExpenseManagement.Domain.Models;
using Assessments.ExpenseManagement.Infrastructure;
using Assessments.ExpenseManagement.Infrastructure.Repositories;
using Assessments.ExpenseManagement.IntegrationTests.Fixtures;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Assessments.ExpenseManagement.IntegrationTests.Abstractions;

public abstract class TestBase(TestContainerFactory factory) : IAsyncLifetime
{
    private DbConnection Connection { get; } = new SqlConnection(factory.ConnectionString);
    protected ExpenseDbContext Context { get; private set; } = null!;

    public virtual async Task InitializeAsync()
    {
        await Connection.OpenAsync();
        Context = new ExpenseDbContext(
            new DbContextOptionsBuilder<ExpenseDbContext>().UseSqlServer(Connection).Options);

        CategoryReadRepository = new ReadRepository<Category>(Context);
        CurrencyReadRepository = new ReadRepository<Currency>(Context);
        ExpenseReadRepository = new ReadRepository<Expense>(Context);
        ExpenseWriteRepository = new WriteRepository<Expense>(Context);
        UserReadRepository = new ReadRepository<User>(Context);
    }

    public virtual async Task DisposeAsync()
    {
        await Context.DisposeAsync();
        await Connection.DisposeAsync();
    }

    #region Repositories

    protected IReadRepository<Category> CategoryReadRepository { get; private set; } = null!;
    protected IReadRepository<Currency> CurrencyReadRepository { get; private set; } = null!;
    protected IReadRepository<Expense> ExpenseReadRepository { get; private set; } = null!;
    protected IWriteRepository<Expense> ExpenseWriteRepository { get; private set; } = null!;
    protected IReadRepository<User> UserReadRepository { get; private set; } = null!;

    #endregion
}
