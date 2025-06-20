using System.Net;
using System.Text;
using Assessments.ExpenseManagement.Domain.Constants;

namespace Assessments.ExpenseManagement.Domain.Exceptions;

public partial class DomainException
{
    public static DomainException CurrencyNotFoundByCode(string code) =>
        new(string.Format(null, CompositeFormat.Parse(ErrorMessages.CurrencyNotFound), code))
        {
            StatusCode = HttpStatusCode.NotFound
        };
}
