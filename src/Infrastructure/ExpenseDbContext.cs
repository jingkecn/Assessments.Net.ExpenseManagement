using System.Diagnostics;
using Assessments.ExpenseManagement.Domain.Abstractions;
using Assessments.ExpenseManagement.Domain.Contracts;
using Assessments.ExpenseManagement.Domain.Models;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Assessments.ExpenseManagement.Infrastructure;

public sealed class ExpenseDbContext(DbContextOptions options, IMediator mediator) : DbContext(options), IUnitOfWork
{
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Currency> Currencies => Set<Currency>();
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Expense> Expenses => Set<Expense>();
    public DbSet<Receipt> Receipts => Set<Receipt>();

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        // Dispatch Domain Events collection.
        // Choices:
        // A) Right BEFORE committing data (EF SaveChanges) into the DB will make a single transaction including
        // side effects from the domain event handlers which are using the same DbContext with "InstancePerLifetimeScope" or "scoped" lifetime
        // B) Right AFTER committing data (EF SaveChanges) into the DB will make multiple transactions.
        // We will need to handle eventual consistency and compensatory actions in case of failures in any of the Handlers.
        var domainEntities =
            ChangeTracker.Entries<Entity>().Where(e => e.Entity.DomainEvents.Count is not 0).ToList();
        var domainEvents = domainEntities.SelectMany(e => e.Entity.DomainEvents).ToList();
        domainEntities.ToList().ForEach(e => e.Entity.ClearDomainEvents());
        foreach (var domainEvent in domainEvents)
        {
            await mediator.Publish(domainEvent, cancellationToken);
        }

        // After executing this line all the changes (from the Command Handler and Domain Event Handlers)
        // performed through the DbContext will be committed
        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        Debug.WriteLine(modelBuilder.Model.ToDebugString());
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ExpenseDbContext).Assembly);
    }
}
