using System.Net;
using Assessments.ExpenseManagement.Domain.Events;
using Assessments.ExpenseManagement.Domain.Exceptions;
using Assessments.ExpenseManagement.Domain.Models;
using Assessments.ExpenseManagement.Infrastructure.Presets;
using Shouldly;

namespace Assessments.ExpenseManagement.UnitTests.Domain;

public sealed class ExpenseTests
{
    [Fact]
    public void Create_Should_ReturnExpenseWithDraftStatus()
    {
        // Arrange
        const decimal amount = 100m;
        var category = Category.Create("Travel");
        var currency = CurrencyPresets.CNY;
        var executionDate = DateOnly.FromDateTime(DateTime.Now);
        var receipts = new List<Receipt> { Receipt.Create("Travel expenses for the month", "receipt1.jpg") };

        var employee = Employee.Create("John", "Doe", currency);

        // Act
        var expense = Expense.Create(employee, amount, category, currency, executionDate, receipts);

        // Assert
        expense.ShouldNotBeNull();
        expense.Id.ShouldNotBe(Guid.Empty);
        expense.Amount.ShouldBe(amount);
        expense.CategoryId.ShouldBe(category.Id);
        expense.CurrencyId.ShouldBe(currency.Id);
        expense.EmployeeId.ShouldBe(employee.Id);
        expense.EmployeeInfo.ShouldBeEquivalentTo(new EmployeeInfo
        {
            FirstName = employee.FirstName, LastName = employee.LastName
        });
        expense.ExecutionDate.ShouldBe(executionDate);
        expense.Receipts.ShouldBeEquivalentTo(receipts.AsReadOnly());
        expense.Status.ShouldBe(Status.Draft);
        expense.SubmissionDate.ShouldBe(DateOnly.MinValue);

        expense.DomainEvents.ShouldBeEmpty();
    }

    [Fact]
    public void Create_Should_ReturnTransientExpenseWithDraftStatus_WhenTransient()
    {
        // Arrange
        const decimal amount = 100m;
        var category = Category.Create("Travel");
        var currency = CurrencyPresets.CNY;
        var executionDate = DateOnly.FromDateTime(DateTime.Now);
        var receipts = new List<Receipt> { Receipt.Create("Travel expenses for the month", "receipt1.jpg") };

        var employee = Employee.Create("John", "Doe", currency);

        // Act
        var expense = Expense.Create(employee, amount, category, currency, executionDate, receipts, true);

        // Assert
        expense.ShouldNotBeNull();
        expense.Id.ShouldBe(Guid.Empty);
        expense.Amount.ShouldBe(amount);
        expense.CategoryId.ShouldBe(category.Id);
        expense.CurrencyId.ShouldBe(currency.Id);
        expense.EmployeeId.ShouldBe(employee.Id);
        expense.EmployeeInfo.ShouldBeEquivalentTo(new EmployeeInfo
        {
            FirstName = employee.FirstName, LastName = employee.LastName
        });
        expense.ExecutionDate.ShouldBe(executionDate);
        expense.Receipts.ShouldBeEquivalentTo(receipts.AsReadOnly());
        expense.Status.ShouldBe(Status.Draft);
        expense.SubmissionDate.ShouldBe(DateOnly.MinValue);

        expense.DomainEvents.ShouldBeEmpty();
    }

    [Fact]
    public void AddReceipt_Should_AddReceiptToExpense()
    {
        // Arrange
        const decimal amount = 100m;
        var category = Category.Create("Travel");
        var currency = CurrencyPresets.CNY;
        var employee = Employee.Create("John", "Doe", currency);
        var executionDate = DateOnly.FromDateTime(DateTime.Now);
        var expense = Expense.Create(employee, amount, category, currency, executionDate, []);
        var receipt = Receipt.Create("Travel expenses for the month", "receipt1.jpg");

        // Act
        expense.AddReceipt(receipt);

        // Assert
        expense.Id.ShouldNotBe(Guid.Empty);
        expense.Amount.ShouldBe(amount);
        expense.CategoryId.ShouldBe(category.Id);
        expense.CurrencyId.ShouldBe(currency.Id);
        expense.EmployeeId.ShouldBe(employee.Id);
        expense.EmployeeInfo.ShouldBeEquivalentTo(new EmployeeInfo
        {
            FirstName = employee.FirstName, LastName = employee.LastName
        });
        expense.ExecutionDate.ShouldBe(executionDate);
        expense.Receipts.ShouldContain(receipt);
        expense.Status.ShouldBe(Status.Draft);
        expense.SubmissionDate.ShouldBe(DateOnly.MinValue);

        expense.DomainEvents.ShouldContain(e => e is ReceiptAddedEvent);
    }

    [Fact]
    public void RemoveReceipt_Should_RemoveReceiptFromExpense()
    {
        // Arrange
        const decimal amount = 100m;
        var category = Category.Create("Travel");
        var currency = CurrencyPresets.CNY;
        var employee = Employee.Create("John", "Doe", currency);
        var executionDate = DateOnly.FromDateTime(DateTime.Now);
        var receipt = Receipt.Create("Travel expenses for the month", "receipt1.jpg");
        var expense = Expense.Create(employee, amount, category, currency, executionDate, [receipt]);

        // Act
        expense.RemoveReceipt(receipt);

        // Assert
        expense.Id.ShouldNotBe(Guid.Empty);
        expense.Amount.ShouldBe(amount);
        expense.CategoryId.ShouldBe(category.Id);
        expense.CurrencyId.ShouldBe(currency.Id);
        expense.EmployeeId.ShouldBe(employee.Id);
        expense.EmployeeInfo.ShouldBeEquivalentTo(new EmployeeInfo
        {
            FirstName = employee.FirstName, LastName = employee.LastName
        });
        expense.ExecutionDate.ShouldBe(executionDate);
        expense.Receipts.ShouldNotContain(receipt);
        expense.Status.ShouldBe(Status.Draft);
        expense.SubmissionDate.ShouldBe(DateOnly.MinValue);

        expense.DomainEvents.ShouldContain(e => e is ReceiptRemovedEvent);
    }

    [Fact]
    public void Cancel_Should_ChangeStatusToCancelled()
    {
        // Arrange
        const decimal amount = 100m;
        var category = Category.Create("Travel");
        var currency = CurrencyPresets.CNY;
        var employee = Employee.Create("John", "Doe", currency);
        var executionDate = DateOnly.FromDateTime(DateTime.Now);
        var receipt = Receipt.Create("Travel expenses for the month", "receipt1.jpg");
        var expense = Expense.Create(employee, amount, category, currency, executionDate, [receipt]);

        // Act
        expense.Cancel();

        // Assert
        expense.Id.ShouldNotBe(Guid.Empty);
        expense.Amount.ShouldBe(amount);
        expense.CategoryId.ShouldBe(category.Id);
        expense.CurrencyId.ShouldBe(currency.Id);
        expense.EmployeeId.ShouldBe(employee.Id);
        expense.EmployeeInfo.ShouldBeEquivalentTo(new EmployeeInfo
        {
            FirstName = employee.FirstName, LastName = employee.LastName
        });
        expense.ExecutionDate.ShouldBe(executionDate);
        expense.Receipts.ShouldContain(receipt);
        expense.Status.ShouldBe(Status.Cancelled);
        expense.SubmissionDate.ShouldBe(DateOnly.MinValue);

        expense.DomainEvents.Count.ShouldBe(1);
        expense.DomainEvents.OfType<ExpenseCancelledEvent>()
            .ShouldContain(e => e.EmployeeId == employee.Id && e.ExpenseId == expense.Id);
    }

    [Fact]
    public void Cancel_Should_ThrowDomainException_When_StatusIsInvalid()
    {
        // Arrange
        const decimal amount = 100m;
        var category = Category.Create("Travel");
        var currency = CurrencyPresets.CNY;
        var employee = Employee.Create("John", "Doe", currency);
        var executionDate = DateOnly.FromDateTime(DateTime.Now);
        var receipt = Receipt.Create("Travel expenses for the month", "receipt1.jpg");
        var expense = Expense.Create(employee, amount, category, currency, executionDate, [receipt]);

        expense.Cancel(); // Change status to 'Cancelled'.

        // Act
        var action = () => expense.Cancel();

        // Assert
        var exception = action.ShouldThrow<DomainException>();
        exception.StatusCode.ShouldBe(HttpStatusCode.PreconditionFailed);
        exception.Message.ShouldBe(DomainException
            .ExpenseInvalidStatusToPerformAction(Status.Cancelled, nameof(Expense.Cancel)).Message);
    }

    [Fact]
    public void Submit_Should_ChangeStatusToSubmittedAndSetSubmissionDate()
    {
        // Arrange
        const decimal amount = 100m;
        var category = Category.Create("Travel");
        var currency = CurrencyPresets.CNY;
        var employee = Employee.Create("John", "Doe", currency);
        var executionDate = DateOnly.FromDateTime(DateTime.Now);
        var receipt = Receipt.Create("Travel expenses for the month", "receipt1.jpg");
        var expense = Expense.Create(employee, amount, category, currency, executionDate, [receipt]);

        // Act
        expense.Submit();

        // Assert
        expense.Id.ShouldNotBe(Guid.Empty);
        expense.Amount.ShouldBe(amount);
        expense.CategoryId.ShouldBe(category.Id);
        expense.CurrencyId.ShouldBe(currency.Id);
        expense.EmployeeId.ShouldBe(employee.Id);
        expense.EmployeeInfo.ShouldBeEquivalentTo(new EmployeeInfo
        {
            FirstName = employee.FirstName, LastName = employee.LastName
        });
        expense.ExecutionDate.ShouldBe(executionDate);
        expense.Receipts.ShouldContain(receipt);
        expense.Status.ShouldBe(Status.Submitted);
        expense.SubmissionDate.ShouldNotBe(DateOnly.MinValue);

        expense.DomainEvents.Count.ShouldBe(1);
        expense.DomainEvents.OfType<ExpenseSubmittedEvent>()
            .ShouldContain(e => e.EmployeeId == employee.Id && e.ExpenseId == expense.Id);
    }

    [Fact]
    public void Submit_Should_ThrowDomainException_When_StatusIsInvalid()
    {
        // Arrange
        const decimal amount = 100m;
        var category = Category.Create("Travel");
        var currency = CurrencyPresets.CNY;
        var employee = Employee.Create("John", "Doe", currency);
        var executionDate = DateOnly.FromDateTime(DateTime.Now);
        var receipt = Receipt.Create("Travel expenses for the month", "receipt1.jpg");
        var expense = Expense.Create(employee, amount, category, currency, executionDate, [receipt]);

        expense.Submit(); // Change status to 'Submitted'.

        // Act
        var action = () => expense.Submit();

        // Assert
        var exception = action.ShouldThrow<DomainException>();
        exception.StatusCode.ShouldBe(HttpStatusCode.PreconditionFailed);
        exception.Message.ShouldBe(DomainException
            .ExpenseInvalidStatusToPerformAction(Status.Submitted, nameof(Expense.Submit)).Message);
    }
}
