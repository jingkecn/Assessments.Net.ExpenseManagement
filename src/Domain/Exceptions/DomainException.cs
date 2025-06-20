using System.Net;

namespace Assessments.ExpenseManagement.Domain.Exceptions;

public partial class DomainException : Exception
{
    private DomainException() { }
    private DomainException(string message) : base(message) { }
    private DomainException(string message, Exception innerException) : base(message, innerException) { }

    public HttpStatusCode StatusCode { get; private init; }
}
