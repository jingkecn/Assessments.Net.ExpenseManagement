using Assessments.ExpenseManagement.Domain.Models;

namespace Assessments.ExpenseManagement.Infrastructure.Models;

public static class CategoryPresets
{
    public static readonly Category Hotel =
        Category.Create(Guid.Parse("0f3043bb-4be9-4899-9cc5-d8250fad73df"), nameof(Hotel));

    public static readonly Category Restaurant =
        Category.Create(Guid.Parse("39c3d33b-cd7e-4f64-abb4-465f91baef1b"), nameof(Restaurant));

    public static readonly Category Travel =
        Category.Create(Guid.Parse("3afbdad9-6c02-47ac-aa87-4a85f3a1a1ca"), nameof(Travel));

    public static IEnumerable<Category> PredefinedCategories
    {
        get
        {
            yield return Hotel;
            yield return Restaurant;
            yield return Travel;
        }
    }
}
