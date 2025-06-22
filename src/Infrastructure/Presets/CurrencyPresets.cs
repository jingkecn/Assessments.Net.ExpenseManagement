using System.Diagnostics.CodeAnalysis;
using Assessments.ExpenseManagement.Domain.Models;

namespace Assessments.ExpenseManagement.Infrastructure.Presets;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class CurrencyPresets
{
    public static readonly Currency CNY = Currency.Create(
        Guid.Parse("2a20ef19-6a03-48ac-9b42-6a0b2cdb7c80"), "CNY", "Chinese Yuan", "¥");

    public static readonly Currency EUR = Currency.Create(
        Guid.Parse("4b21486c-9517-494b-a324-6132f32fb1c9"), "EUR", "Euro", "€");

    public static readonly Currency USD = Currency.Create(
        Guid.Parse("fdb8bbd1-6322-47a4-bb0f-4b78f15fda78"), "USD", "United States Dollar", "$");

    public static IEnumerable<Currency> PredefinedCurrencies
    {
        get
        {
            yield return CNY;
            yield return EUR;
            yield return USD;
        }
    }
}
