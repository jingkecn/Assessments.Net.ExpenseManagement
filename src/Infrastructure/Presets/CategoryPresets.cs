using Assessments.ExpenseManagement.Domain.Models;

namespace Assessments.ExpenseManagement.Infrastructure.Presets;

public static class CategoryPresets
{
    public static readonly Category Hotel =
        Category.Create(Guid.Parse("5405fde5-ee32-497f-905e-6926ad66c233"), nameof(Hotel));

    public static readonly Category Restaurant =
        Category.Create(Guid.Parse("46af734f-2312-4351-96cd-9705a4cf90eb"), nameof(Restaurant));

    public static readonly Category Travel =
        Category.Create(Guid.Parse("cde30ccd-b5f0-442b-bcd0-c73cdd036f00"), nameof(Travel));

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
