using System.Diagnostics.CodeAnalysis;
using Assessments.ExpenseManagement.Domain.Models;

namespace Assessments.ExpenseManagement.Infrastructure.Models;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class CurrencyPresets
{
    public static readonly Currency EUR =
        Currency.Create(Guid.Parse("c97190a9-6d94-40f4-bb67-57e658678511"), nameof(EUR), '€');

    public static readonly Currency USD =
        Currency.Create(Guid.Parse("ff724b35-1947-4654-9700-e25d2b31dc7c"), nameof(USD), '$');

    internal static IEnumerable<Currency> PredefinedCurrencies
    {
        get
        {
            yield return EUR;
            yield return USD;
        }
    }
}
