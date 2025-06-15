using System.Net;
using Assessments.ExpenseManagement.Domain.Models;

namespace Assessments.ExpenseManagement.Domain.Exceptions;

public partial class DomainException
{
    public static DomainException CategoryNotFoundByName(string name) =>
        new($"{nameof(Category)} [{nameof(Category.Name)}={name}] not found.") { StatusCode = HttpStatusCode.NotFound };
}
