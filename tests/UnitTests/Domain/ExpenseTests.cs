using Assessments.ExpenseManagement.Domain.Events;
using Assessments.ExpenseManagement.Domain.Exceptions;
using Assessments.ExpenseManagement.Domain.Models;
using Shouldly;

namespace Assessments.ExpenseManagement.UnitTests.Domain;

public sealed class ExpenseTests
{
    [Fact]
    public void Create_Should_ReturnExpenseWithDraftStatus()
    {
        // Arrange
        var amount = new Amount { Value = 100.00m };
        var category = Category.Create("Travel");
        var currency = Currency.Create("CNY", "Chinese Yuan", "¥");
        var receipts = new List<Receipt> { Receipt.Create("Travel expenses for the month", "receipt1.jpg") };

        var employee = Employee.Create("John", "Doe", currency.Code);

        // Act
        var expense = Expense.Create(employee, amount, category, currency, receipts);

        // Assert
        expense.ShouldNotBeNull();
        expense.Id.ShouldNotBe(Guid.Empty);
        expense.Amount.ShouldBe(amount);
        expense.Category.ShouldBe(category);
        expense.Currency.ShouldBe(currency);
        expense.Employee.ShouldBe(employee);
        expense.Receipts.ShouldBeEquivalentTo(receipts.AsReadOnly());
        expense.Status.ShouldBe(Status.Draft);
        expense.SubmissionDate.ShouldBe(DateOnly.MinValue);

        expense.DomainEvents.ShouldBeEmpty();
    }

    [Fact]
    public void Create_Should_ReturnTransientExpenseWithDraftStatus_WhenTransient()
    {
        // Arrange
        var amount = new Amount { Value = 100.00m };
        var category = Category.Create("Travel");
        var currency = Currency.Create("CNY", "Chinese Yuan", "¥");
        var receipts = new List<Receipt> { Receipt.Create("Travel expenses for the month", "receipt1.jpg") };

        var employee = Employee.Create("John", "Doe", currency.Code);

        // Act
        var expense = Expense.Create(employee, amount, category, currency, receipts, true);

        // Assert
        expense.ShouldNotBeNull();
        expense.Id.ShouldBe(Guid.Empty);
        expense.Amount.ShouldBe(amount);
        expense.Category.ShouldBe(category);
        expense.Currency.ShouldBe(currency);
        expense.Employee.ShouldBe(employee);
        expense.Receipts.ShouldBeEquivalentTo(receipts.AsReadOnly());
        expense.Status.ShouldBe(Status.Draft);
        expense.SubmissionDate.ShouldBe(DateOnly.MinValue);

        expense.DomainEvents.ShouldBeEmpty();
    }

    [Fact]
    public void AddReceipt_Should_AddReceiptToExpense()
    {
        // Arrange
        var amount = new Amount { Value = 100.00m };
        var category = Category.Create("Travel");
        var currency = Currency.Create("CNY", "Chinese Yuan", "¥");
        var employee = Employee.Create("John", "Doe", currency.Code);
        var expense = Expense.Create(employee, amount, category, currency, []);
        var receipt = Receipt.Create("Travel expenses for the month", "receipt1.jpg");

        // Act
        expense.AddReceipt(receipt);

        // Assert
        expense.Id.ShouldNotBe(Guid.Empty);
        expense.Amount.ShouldBe(amount);
        expense.Category.ShouldBe(category);
        expense.Currency.ShouldBe(currency);
        expense.Employee.ShouldBe(employee);
        expense.Receipts.ShouldContain(receipt);
        expense.Status.ShouldBe(Status.Draft);
        expense.SubmissionDate.ShouldBe(DateOnly.MinValue);

        expense.DomainEvents.ShouldContain(e => e is ReceiptAddedEvent);
    }

    [Fact]
    public void RemoveReceipt_Should_RemoveReceiptFromExpense()
    {
        // Arrange
        var amount = new Amount { Value = 100.00m };
        var category = Category.Create("Travel");
        var currency = Currency.Create("CNY", "Chinese Yuan", "¥");
        var employee = Employee.Create("John", "Doe", currency.Code);
        var receipt = Receipt.Create("Travel expenses for the month", "receipt1.jpg");
        var expense = Expense.Create(employee, amount, category, currency, [receipt]);

        // Act
        expense.RemoveReceipt(receipt);

        // Assert
        expense.Id.ShouldNotBe(Guid.Empty);
        expense.Amount.ShouldBe(amount);
        expense.Category.ShouldBe(category);
        expense.Currency.ShouldBe(currency);
        expense.Employee.ShouldBe(employee);
        expense.Receipts.ShouldNotContain(receipt);
        expense.Status.ShouldBe(Status.Draft);
        expense.SubmissionDate.ShouldBe(DateOnly.MinValue);

        expense.DomainEvents.ShouldContain(e => e is ReceiptRemovedEvent);
    }

    [Fact]
    public void Submit_Should_ChangeStatusToSubmittedAndSetSubmissionDate()
    {
        // Arrange
        var amount = new Amount { Value = 100.00m };
        var category = Category.Create("Travel");
        var currency = Currency.Create("CNY", "Chinese Yuan", "¥");
        var employee = Employee.Create("John", "Doe", currency.Code);
        var receipt = Receipt.Create("Travel expenses for the month", "receipt1.jpg");
        var expense = Expense.Create(employee, amount, category, currency, [receipt]);

        // Act
        expense.Submit();

        // Assert
        expense.Id.ShouldNotBe(Guid.Empty);
        expense.Amount.ShouldBe(amount);
        expense.Category.ShouldBe(category);
        expense.Currency.ShouldBe(currency);
        expense.Employee.ShouldBe(employee);
        expense.Receipts.ShouldContain(receipt);
        expense.Status.ShouldBe(Status.Submitted);
        expense.SubmissionDate.ShouldNotBe(DateOnly.MinValue);

        expense.DomainEvents.Count.ShouldBe(1);
        expense.DomainEvents.ShouldContain(e => e is ExpenseSubmittedEvent);
    }

    [Fact]
    public void Submit_Should_ThrowDomainException_WhenStatusIsNotDraft()
    {
        // Arrange
        var amount = new Amount { Value = 100.00m };
        var category = Category.Create("Travel");
        var currency = Currency.Create("CNY", "Chinese Yuan", "¥");
        var employee = Employee.Create("John", "Doe", currency.Code);
        var receipt = Receipt.Create("Travel expenses for the month", "receipt1.jpg");
        var expense = Expense.Create(employee, amount, category, currency, [receipt]);
        expense.Submit(); // Change status to Submitted

        // Act
        var action = () => expense.Submit();

        // Assert
        action.ShouldThrow<DomainException>()
            .Message.ShouldBe(DomainException.InvalidStatus(Status.Submitted, nameof(expense.Submit)).Message);
    }

    [Fact]
    public void Validate_Should_ThrowDomainException_WhenAmountIsInvalid()
    {
        // Arrange
        var amount = new Amount { Value = -100.00m }; // Invalid amount
        var category = Category.Create("Travel");
        var currency = Currency.Create("CNY", "Chinese Yuan", "¥");
        var employee = Employee.Create("John", "Doe", currency.Code);
        var receipt = Receipt.Create("Travel expenses for the month", "receipt1.jpg");
        var expense = Expense.Create(employee, amount, category, currency, [receipt]);

        // Act
        var action = () => expense.Validate();

        // Assert
        action.ShouldThrow<DomainException>().Message.ShouldBe(DomainException.InvalidAmount(amount).Message);
    }

    [Fact]
    public void Validate_Should_ThrowDomainException_WhenCategoryIsNotAllowedForEmployee()
    {
        // Arrange
        var amount = new Amount { Value = 100.00m };
        var category = Category.Create("Travel");
        var currency = Currency.Create("CNY", "Chinese Yuan", "¥");
        var employee = Employee.Create("John", "Doe", currency.Code, true);
        var receipt = Receipt.Create("Travel expenses for the month", "receipt1.jpg");
        var expense = Expense.Create(employee, amount, category, currency, [receipt]);

        // Act
        var action = () => expense.Validate();

        // Assert
        action.ShouldThrow<DomainException>()
            .Message.ShouldBe(DomainException.EmployeeNotAllowed(employee.Id, category).Message);
    }

    [Fact]
    public void Validate_Should_ThrowDomainException_WhenNoReceiptsAreAttached()
    {
        // Arrange
        var amount = new Amount { Value = 100.00m };
        var category = Category.Create("Travel");
        var currency = Currency.Create("CNY", "Chinese Yuan", "¥");
        var employee = Employee.Create("John", "Doe", currency.Code);
        var expense = Expense.Create(employee, amount, category, currency, []);

        // Act
        var action = () => expense.Validate();

        // Assert
        action.ShouldThrow<DomainException>()
            .Message.ShouldBe(DomainException.NoReceipts().Message);
    }

    [Fact]
    public void Validate_Should_ThrowDomainException_WhenInvalidReceiptsAreAttached()
    {
        // Arrange
        var amount = new Amount { Value = 100.00m };
        var category = Category.Create("Travel");
        var currency = Currency.Create("CNY", "Chinese Yuan", "¥");
        var employee = Employee.Create("John", "Doe", currency.Code);
        var receipt =
            Receipt.Create("Travel expenses for the month", string.Empty); // Invalid receipt with invalid file.
        var expense = Expense.Create(employee, amount, category, currency, [receipt]);

        // Act
        var action = () => expense.Validate();

        // Assert
        action.ShouldThrow<DomainException>()
            .Message.ShouldBe(DomainException.InvalidReceipts([receipt]).Message);
    }
}
