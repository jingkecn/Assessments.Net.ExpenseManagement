using System.Reflection;
using Assessments.ExpenseManagement.Command.Api.Endpoints;
using Assessments.ExpenseManagement.Domain.Abstractions;
using Assessments.ExpenseManagement.Query.Api.Endpoints;
using ApplicationDependencyInjection = Assessments.ExpenseManagement.Application.Extensions.DependencyInjection;
using InfrastructureDependencyInjection = Assessments.ExpenseManagement.Infrastructure.Extensions.DependencyInjection;

namespace Assessments.ExpenseManagement.ArchitectureTests.Abstractions;

public abstract class TestBase
{
    private const string RootNamespace = "Assessments.ExpenseManagement";
    protected const string ApplicationNamespace = $"{RootNamespace}.Application";
    protected const string InfrastructureNamespace = $"{RootNamespace}.Infrastructure";
    protected const string CommandApiNamespace = $"{RootNamespace}.Command.Api";
    protected const string QueryApiNamespace = $"{RootNamespace}.Query.Api";

    protected static readonly Assembly ApplicationAssembly = typeof(ApplicationDependencyInjection).Assembly;
    protected static readonly Assembly DomainAssembly = typeof(Entity).Assembly;
    protected static readonly Assembly InfrastructureAssembly = typeof(InfrastructureDependencyInjection).Assembly;

    protected static readonly Assembly CommandApiAssembly = typeof(ExpenseCommandEndpoints).Assembly;
    protected static readonly Assembly QueryApiAssembly = typeof(ExpenseQueryEndpoints).Assembly;
}
