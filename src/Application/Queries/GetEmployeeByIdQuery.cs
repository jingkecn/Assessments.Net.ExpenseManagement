using System.ComponentModel.DataAnnotations;
using Assessments.ExpenseManagement.Application.Models;
using Mediator;

namespace Assessments.ExpenseManagement.Application.Queries;

public record GetEmployeeByIdQuery([property: Required] Guid Id) : IQuery<EmployeeView>;
