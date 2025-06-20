using System.Net;
using System.Text;
using Assessments.ExpenseManagement.Domain.Constants;

namespace Assessments.ExpenseManagement.Domain.Exceptions;

public partial class DomainException
{
    public static DomainException CategoryNotFoundByName(string name) =>
        new(string.Format(null, CompositeFormat.Parse(ErrorMessages.CategoryNotFound), name))
        {
            StatusCode = HttpStatusCode.NotFound
        };
}
