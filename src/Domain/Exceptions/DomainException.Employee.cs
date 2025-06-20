using System.Net;
using System.Text;
using Assessments.ExpenseManagement.Domain.Constants;

namespace Assessments.ExpenseManagement.Domain.Exceptions;

public partial class DomainException
{
    public static DomainException EmployeeNotFoundById(Guid id) =>
        new(string.Format(null, CompositeFormat.Parse(ErrorMessages.EmployeeNotFound), id))
        {
            StatusCode = HttpStatusCode.NotFound
        };
}
