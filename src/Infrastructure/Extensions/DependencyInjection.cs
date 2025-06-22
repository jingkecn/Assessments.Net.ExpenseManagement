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
            .AddScoped<IReadRepository<Employee>, ReadRepository<Employee>>()
            .AddScoped<IReadRepository<Expense>, ReadRepository<Expense>>()
            .AddScoped<IReadRepository<Receipt>, ReadRepository<Receipt>>();

    private static IServiceCollection AddWriteRepositories(this IServiceCollection services) =>
        services
            .AddScoped<IWriteRepository<Category>, WriteRepository<Category>>()
            .AddScoped<IWriteRepository<Currency>, WriteRepository<Currency>>()
            .AddScoped<IWriteRepository<Employee>, WriteRepository<Employee>>()
            .AddScoped<IWriteRepository<Expense>, WriteRepository<Expense>>()
            .AddScoped<IWriteRepository<Receipt>, WriteRepository<Receipt>>();
}
