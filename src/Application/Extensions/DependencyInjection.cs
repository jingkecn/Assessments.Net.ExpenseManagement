using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Assessments.ExpenseManagement.Application.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services) =>
        services
            .AddMediator(option => option.ServiceLifetime = ServiceLifetime.Scoped)
            .AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
}
