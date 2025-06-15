using Assessments.ExpenseManagement.Domain.Contracts;
using Assessments.ExpenseManagement.Domain.Models;
using NSubstitute;

namespace Assessments.ExpenseManagement.UnitTests.Abstractions;

public abstract class TestBase
{
    protected IReadRepository<Category> CategoryReadRepository { get; } =
        Substitute.For<IReadRepository<Category>>();

    protected IReadRepository<Currency> CurrencyReadRepository { get; } =
        Substitute.For<IReadRepository<Currency>>();

    protected IReadRepository<Expense> ExpenseReadRepository { get; } =
        Substitute.For<IReadRepository<Expense>>();

    protected IWriteRepository<Expense> ExpenseWriteRepository { get; } =
        Substitute.For<IWriteRepository<Expense>>();

    protected IReadRepository<User> UserReadRepository { get; } =
        Substitute.For<IReadRepository<User>>();
}
