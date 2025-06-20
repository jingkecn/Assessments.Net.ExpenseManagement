using Assessments.ExpenseManagement.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Assessments.ExpenseManagement.Infrastructure.Configurations;

public sealed class CurrencyEntityTypeConfiguration : IEntityTypeConfiguration<Currency>
{
    private static IEnumerable<Currency> PredefinedCurrencies
    {
        get
        {
            yield return Currency.Create(
                Guid.Parse("2a20ef19-6a03-48ac-9b42-6a0b2cdb7c80"), "CNY", "Chinese Yuan", "¥");
            yield return Currency.Create(
                Guid.Parse("4b21486c-9517-494b-a324-6132f32fb1c9"), "EUR", "Euro", "€");
            yield return Currency.Create(
                Guid.Parse("fdb8bbd1-6322-47a4-bb0f-4b78f15fda78"), "USD", "United States Dollar", "$");
        }
    }

    public void Configure(EntityTypeBuilder<Currency> builder)
    {
        builder.HasKey(c => c.Id);
        builder.HasData(PredefinedCurrencies);
    }
}
