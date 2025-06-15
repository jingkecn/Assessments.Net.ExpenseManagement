using Assessments.ExpenseManagement.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Assessments.ExpenseManagement.Infrastructure.Configurations;

public sealed class ExpenseEntityTypeConfiguration : IEntityTypeConfiguration<Expense>
{
    public void Configure(EntityTypeBuilder<Expense> builder)
    {
        builder.HasOne(e => e.Category)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction); // No cascade delete
        builder.HasOne(e => e.Currency)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction); // No cascade delete
        builder.HasOne(e => e.User)
            .WithMany()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
