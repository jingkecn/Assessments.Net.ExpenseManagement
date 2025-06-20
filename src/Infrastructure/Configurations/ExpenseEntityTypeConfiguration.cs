using Assessments.ExpenseManagement.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Assessments.ExpenseManagement.Infrastructure.Configurations;

public sealed class ExpenseEntityTypeConfiguration : IEntityTypeConfiguration<Expense>
{
    public void Configure(EntityTypeBuilder<Expense> builder)
    {
        builder.ComplexProperty(e => e.Amount);
        builder.HasOne(e => e.Category);
        builder.HasOne(e => e.Currency);
        builder.HasOne(e => e.Employee)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);
        builder.HasMany(e => e.Receipts)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
