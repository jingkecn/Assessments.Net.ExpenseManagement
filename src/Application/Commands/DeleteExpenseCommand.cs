using System.ComponentModel.DataAnnotations;
using Mediator;

namespace Assessments.ExpenseManagement.Application.Commands;

public sealed record DeleteExpenseCommand([property: Required] Guid Id) : ICommand<bool>;
