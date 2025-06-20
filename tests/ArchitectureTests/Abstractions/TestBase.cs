using System.Reflection;
using Assessments.ExpenseManagement.Domain.Abstractions;

namespace Assessments.ExpenseManagement.ArchitectureTests.Abstractions;

public abstract class TestBase
{
    protected static readonly Assembly DomainAssembly = typeof(Entity).Assembly;
}
