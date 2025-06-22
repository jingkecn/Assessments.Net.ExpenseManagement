using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using Assessments.ExpenseManagement.Domain.Constants;
using Assessments.ExpenseManagement.Domain.Models;

namespace Assessments.ExpenseManagement.Domain.Exceptions;

public partial class DomainException
{
    public static DomainException ExpenseAlreadyExists(
        Guid employeeId,
        string categoryName,
        string displayAmount,
        DateOnly executionDate) =>
        new(string.Format(
            null,
            CompositeFormat.Parse(ErrorMessages.ExpenseAlreadyExists),
            employeeId,
            categoryName,
            displayAmount,
            executionDate)) { StatusCode = HttpStatusCode.Conflict };

    public static DomainException ExpenseInvalidCurrencyForEmployee(string expenseCurrencyCode, Employee employee) =>
        new(string.Format(
            null,
            CompositeFormat.Parse(ErrorMessages.ExpenseInvalidCurrencyForEmployee),
            expenseCurrencyCode,
            employee.Id,
            employee.Currency!.Code)) { StatusCode = HttpStatusCode.PreconditionFailed };

    public static DomainException ExpenseInvalidStatusToPerformAction(
        Status expenseStatus,
        [CallerMemberName] string? action = null) =>
        new(string.Format(
            null,
            CompositeFormat.Parse(ErrorMessages.ExpenseInvalidStatusToPerformAction),
            expenseStatus,
            action)) { StatusCode = HttpStatusCode.PreconditionFailed };

    public static DomainException ExpenseNotFoundById(Guid id) =>
        new(string.Format(null, CompositeFormat.Parse(ErrorMessages.ExpenseNotFound), id))
        {
            StatusCode = HttpStatusCode.NotFound
        };

    public static DomainException ExpenseNotFoundByIdForEmployee(Guid expenseId, Guid employeeId) =>
        new(string.Format(null, CompositeFormat.Parse(ErrorMessages.ExpenseNotFoundForEmployee), expenseId, employeeId))
        {
            StatusCode = HttpStatusCode.NotFound
        };

    public static DomainException ExpenseReceiptNotProvided() =>
        new(ErrorMessages.ExpenseReceiptNotProvided) { StatusCode = HttpStatusCode.PreconditionFailed };
}
