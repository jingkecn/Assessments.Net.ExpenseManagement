using System.Diagnostics;
using Assessments.ExpenseManagement.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Assessments.ExpenseManagement.Infrastructure;

public sealed class ExpenseManagementContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Currency> Currencies => Set<Currency>();
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Expense> Expenses => Set<Expense>();
    public DbSet<Receipt> Receipts => Set<Receipt>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        Debug.WriteLine(modelBuilder.Model.ToDebugString());
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ExpenseManagementContext).Assembly);
    }
}
