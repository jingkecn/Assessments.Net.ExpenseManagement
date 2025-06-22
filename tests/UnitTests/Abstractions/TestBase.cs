using Assessments.ExpenseManagement.Domain.Contracts;
using Assessments.ExpenseManagement.Domain.Models;
using NSubstitute;

namespace Assessments.ExpenseManagement.UnitTests.Abstractions;

public abstract class TestBase
{
    protected TestBase()
    {
        CategoryWriteRepository.UnitOfWork.Returns(Substitute.For<IUnitOfWork>());
        CurrencyWriteRepository.UnitOfWork.Returns(Substitute.For<IUnitOfWork>());
        EmployeeWriteRepository.UnitOfWork.Returns(Substitute.For<IUnitOfWork>());
        ExpenseWriteRepository.UnitOfWork.Returns(Substitute.For<IUnitOfWork>());
        ReceiptWriteRepository.UnitOfWork.Returns(Substitute.For<IUnitOfWork>());
    }

    protected IReadRepository<Category> CategoryReadRepository { get; } =
        Substitute.For<IReadRepository<Category>>();

    protected IWriteRepository<Category> CategoryWriteRepository { get; } =
        Substitute.For<IWriteRepository<Category>>();

    protected IReadRepository<Currency> CurrencyReadRepository { get; } =
        Substitute.For<IReadRepository<Currency>>();

    protected IWriteRepository<Currency> CurrencyWriteRepository { get; } =
        Substitute.For<IWriteRepository<Currency>>();

    protected IReadRepository<Employee> EmployeeReadRepository { get; } =
        Substitute.For<IReadRepository<Employee>>();

    protected IWriteRepository<Employee> EmployeeWriteRepository { get; } =
        Substitute.For<IWriteRepository<Employee>>();

    protected IReadRepository<Expense> ExpenseReadRepository { get; } =
        Substitute.For<IReadRepository<Expense>>();

    protected IWriteRepository<Expense> ExpenseWriteRepository { get; } =
        Substitute.For<IWriteRepository<Expense>>();

    protected IReadRepository<Receipt> ReceiptReadRepository { get; } =
        Substitute.For<IReadRepository<Receipt>>();

    protected IWriteRepository<Receipt> ReceiptWriteRepository { get; } =
        Substitute.For<IWriteRepository<Receipt>>();
}
