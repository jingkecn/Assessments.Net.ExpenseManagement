using Assessments.ExpenseManagement.Domain.Models;
using Assessments.ExpenseManagement.Infrastructure.Presets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Assessments.ExpenseManagement.Infrastructure.Configurations;

public sealed class EmployeeEntityTypeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.HasOne(e => e.Currency)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);
        builder.HasData(EmployeePresets.PredefinedEmployees);
    }
}
