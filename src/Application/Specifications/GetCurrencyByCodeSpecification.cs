using Assessments.ExpenseManagement.Application.Abstractions;
using Assessments.ExpenseManagement.Domain.Models;

namespace Assessments.ExpenseManagement.Application.Specifications;

public sealed class GetCurrencyByCodeSpecification(string code) : Specification<Currency>(c => c.Code == code);
