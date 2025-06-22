using Assessments.ExpenseManagement.Domain.Models;

namespace Assessments.ExpenseManagement.Infrastructure.Presets;

public static class EmployeePresets
{
    public static readonly Employee JaneDoe = Employee.Create(
        Guid.Parse("a1b2c3d4-e5f6-7a8b-9c0d-1e2f3a4b5c6d"), "Jane", "Doe", CurrencyPresets.EUR);

    public static readonly Employee JohnDoe = Employee.Create(
        Guid.Parse("d1f8b2c3-4e5f-4a6b-8c9d-0e1f2a3b4c5d"), "John", "Doe", CurrencyPresets.USD);

    public static IEnumerable<Employee> PredefinedEmployees
    {
        get
        {
            yield return JaneDoe;
            yield return JohnDoe;
        }
    }
}
