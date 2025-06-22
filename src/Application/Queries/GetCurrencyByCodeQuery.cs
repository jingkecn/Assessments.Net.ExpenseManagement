using System.ComponentModel.DataAnnotations;
using Assessments.ExpenseManagement.Domain.Models;
using Mediator;

namespace Assessments.ExpenseManagement.Application.Queries;

public record GetCurrencyByCodeQuery([property: Required] string Code) : IQuery<Currency>;
