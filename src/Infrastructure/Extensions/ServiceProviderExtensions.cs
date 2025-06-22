using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Assessments.ExpenseManagement.Infrastructure.Extensions;

public static class ServiceProviderExtensions
{
    public static IServiceProvider InitializeInfrastructure(this IServiceProvider source)
    {
        using var scope = source.CreateScope();
        scope.ServiceProvider.GetRequiredService<ExpenseDbContext>().Database.Migrate();
        return source;
    }
}
