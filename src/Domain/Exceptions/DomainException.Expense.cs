using System.Net;
using Assessments.ExpenseManagement.Domain.Models;

namespace Assessments.ExpenseManagement.Domain.Exceptions;

public partial class DomainException
{
    public static DomainException ExpenseNotFoundById(Guid id) =>
        new($"{nameof(Expense)} [{nameof(Expense.Id)}={id}] not found.") { StatusCode = HttpStatusCode.NotFound };
}
