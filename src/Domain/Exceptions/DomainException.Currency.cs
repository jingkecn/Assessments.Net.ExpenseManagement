using System.Net;
using Assessments.ExpenseManagement.Domain.Models;

namespace Assessments.ExpenseManagement.Domain.Exceptions;

public partial class DomainException
{
    public static DomainException CurrencyNotFoundByCode(string code) =>
        new($"{nameof(Currency)} [{nameof(Currency.Code)}={code}] not found.") { StatusCode = HttpStatusCode.NotFound };
}
