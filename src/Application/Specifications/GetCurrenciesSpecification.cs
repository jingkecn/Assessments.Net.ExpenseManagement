using Assessments.ExpenseManagement.Application.Abstractions;
using Assessments.ExpenseManagement.Domain.Models;

namespace Assessments.ExpenseManagement.Application.Specifications;

public sealed class GetCurrenciesSpecification() : Specification<Currency>(_ => true);
