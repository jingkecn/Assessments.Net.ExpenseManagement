using Assessments.ExpenseManagement.Application.Models;
using Assessments.ExpenseManagement.Domain.Models;

namespace Assessments.ExpenseManagement.Application.Extensions;

public static class EmployeeExtensions
{
    public static EmployeeView ToView(this Employee source) =>
        new(source.FirstName, source.LastName, source.Currency?.Code);

    public static IEnumerable<EmployeeView> ToView(this IEnumerable<Employee> source) =>
        source.Select(e => e.ToView());
}
