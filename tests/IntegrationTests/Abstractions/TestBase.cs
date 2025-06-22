using System.Data.Common;
using Assessments.ExpenseManagement.Domain.Contracts;
using Assessments.ExpenseManagement.Domain.Models;
using Assessments.ExpenseManagement.Infrastructure;
using Assessments.ExpenseManagement.Infrastructure.Repositories;
using Assessments.ExpenseManagement.IntegrationTests.Fixtures;
using Mediator;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NSubstitute;

namespace Assessments.ExpenseManagement.IntegrationTests.Abstractions;

public abstract class TestBase(TestContainerFactory factory) : IAsyncLifetime
{
    private DbConnection Connection { get; } = new SqlConnection(factory.ConnectionString);
    protected ExpenseDbContext Context { get; private set; } = null!;

    public virtual async Task InitializeAsync()
    {
        await Connection.OpenAsync();
        Context = new ExpenseDbContext(
            new DbContextOptionsBuilder<ExpenseDbContext>().UseSqlServer(Connection).Options,
            Substitute.For<IMediator>());

        CategoryReadRepository = new ReadRepository<Category>(Context);
        CurrencyReadRepository = new ReadRepository<Currency>(Context);
        EmployeeReadRepository = new ReadRepository<Employee>(Context);
        ExpenseReadRepository = new ReadRepository<Expense>(Context);
        ExpenseWriteRepository = new WriteRepository<Expense>(Context);
    }

    public async Task DisposeAsync()
    {
        await Context.DisposeAsync();
        await Connection.DisposeAsync();
    }

    #region Repositories

    protected IReadRepository<Category> CategoryReadRepository { get; private set; } = null!;
    protected IReadRepository<Currency> CurrencyReadRepository { get; private set; } = null!;
    protected IReadRepository<Employee> EmployeeReadRepository { get; private set; } = null!;
    protected IReadRepository<Expense> ExpenseReadRepository { get; private set; } = null!;
    protected IWriteRepository<Expense> ExpenseWriteRepository { get; private set; } = null!;

    #endregion
}
