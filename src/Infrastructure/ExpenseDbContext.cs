using System.Diagnostics;
using Assessments.ExpenseManagement.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Assessments.ExpenseManagement.Infrastructure;

public sealed class ExpenseDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Currency> Currencies => Set<Currency>();
    public DbSet<Expense> Expenses => Set<Expense>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        Debug.WriteLine(modelBuilder.Model.ToDebugString());
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ExpenseDbContext).Assembly);
    }
}
