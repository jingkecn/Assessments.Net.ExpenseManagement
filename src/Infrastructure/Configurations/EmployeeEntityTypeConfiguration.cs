using Assessments.ExpenseManagement.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Assessments.ExpenseManagement.Infrastructure.Configurations;

public sealed class EmployeeEntityTypeConfiguration : IEntityTypeConfiguration<Employee>
{
    private static IEnumerable<Employee> PredefinedEmployees
    {
        get
        {
            yield return Employee.Create(
                Guid.Parse("d1f8b2c3-4e5f-4a6b-8c9d-0e1f2a3b4c5d"), "John", "Doe", "CNY");
            yield return Employee.Create(
                Guid.Parse("a1b2c3d4-e5f6-7a8b-9c0d-1e2f3a4b5c6d"), "Jane", "Doe", "EUR");
        }
    }

    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.HasKey(e => e.Id);
        builder.HasData(PredefinedEmployees);
    }
}
