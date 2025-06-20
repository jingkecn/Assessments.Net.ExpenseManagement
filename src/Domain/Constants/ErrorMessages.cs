using Assessments.ExpenseManagement.Domain.Models;

namespace Assessments.ExpenseManagement.Domain.Constants;

public static class ErrorMessages
{
    public const string CategoryNotFound = $"{nameof(Category)} [{nameof(Category.Name)}={{0}}] not found.";
    public const string CurrencyNotFound = $"{nameof(Currency)} [{nameof(Currency.Code)}={{0}}] not found.";
    public const string EmployeeNotFound = $"{nameof(Employee)} [{nameof(Employee.Id)}={{0}}] not found.";
    public const string ExpenseNotFound = $"{nameof(Expense)} [{nameof(Expense.Id)}={{0}}] not found.";

    public const string ExpenseNotFoundForEmployee =
        $"{nameof(Expense)} [{nameof(Expense.Id)}={{0}}] not found for {nameof(Employee)} [{nameof(Employee.Id)}={{0}}].";

    public const string ExpenseAlreadyExists =
        $"{nameof(Expense)} [{nameof(Expense.EmployeeId)}={{0}}, {nameof(Expense.Category)}={{1}}, {nameof(Expense.Amount)}={{2}}, {nameof(Expense.ExecutionDate)}={{3}}] already exists.";

    public const string ExpenseInvalidCurrencyForEmployee =
        $"{nameof(Expense)}: Invalid {nameof(Expense.Currency)} [{nameof(Currency.Code)}={{0}}] for {nameof(Employee)} [{nameof(Employee.Id)}={{1}}, {nameof(Employee.Currency)}={{2}}].";

    public const string ExpenseInvalidAction = $"{nameof(Expense)}: Invalid action. Supported actions: [{{0}}].";

    public const string ExpenseInvalidStatusToPerformAction =
        $"{nameof(Expense)}: Invalid status [{{0}}] to perform action [{{1}}].";

    public const string ExpenseReceiptNotProvided = "Expense receipt not provided";
}
