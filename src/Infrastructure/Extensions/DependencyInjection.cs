using Assessments.ExpenseManagement.Domain.Contracts;
using Assessments.ExpenseManagement.Domain.Models;
using Assessments.ExpenseManagement.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Assessments.ExpenseManagement.Infrastructure.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services) =>
        services
            .AddReadRepositories()
            .AddWriteRepositories();

    private static IServiceCollection AddReadRepositories(this IServiceCollection services) =>
        services
            .AddScoped<IReadRepository<Category>, ReadRepository<Category>>()
            .AddScoped<IReadRepository<Currency>, ReadRepository<Currency>>()
            .AddScoped<IReadRepository<Expense>, ReadRepository<Expense>>()
            .AddScoped<IReadRepository<User>, ReadRepository<User>>();

    private static IServiceCollection AddWriteRepositories(this IServiceCollection services) =>
        services.AddScoped<IWriteRepository<Expense>, WriteRepository<Expense>>();
}
