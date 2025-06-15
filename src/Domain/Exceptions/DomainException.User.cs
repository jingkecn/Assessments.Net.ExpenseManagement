using System.Net;
using Assessments.ExpenseManagement.Domain.Models;

namespace Assessments.ExpenseManagement.Domain.Exceptions;

public partial class DomainException
{
    public static DomainException UserNotFoundById(Guid userId) =>
        new($"{nameof(User)} [{nameof(User.Id)}={userId}] not found.") { StatusCode = HttpStatusCode.NotFound };
}
