using System.ComponentModel.DataAnnotations;
using Mediator;

namespace Assessments.ExpenseManagement.Application.Commands;

public sealed record AddEmployeeCommand(
    [property: Required] string FirstName,
    [property: Required] string LastName,
    [property: Required] string CurrencyCode) : ICommand<Guid>;
