using Assessments.ExpenseManagement.Application.Specifications;
using Assessments.ExpenseManagement.Domain.Models;
using Assessments.ExpenseManagement.Infrastructure.Presets;
using Assessments.ExpenseManagement.IntegrationTests.Abstractions;
using Assessments.ExpenseManagement.IntegrationTests.Fixtures;
using Shouldly;

namespace Assessments.ExpenseManagement.IntegrationTests.Specifications;

[Collection(nameof(TestCollectionFixture))]
public sealed class GetEmployeeExpenseByIdWithAllSpecificationTests(TestContainerFactory factory)
    : TestBaseWithDbMigration(factory)
{
    [Fact]
    public async Task Should_GetEmployeeExpenseByIdWithAll_When_Applied()
    {
        // Arrange
        var employee = EmployeePresets.JohnDoe;
        var expected = Expense.Create(
            employee,
            100M,
            CategoryPresets.Hotel,
            CurrencyPresets.USD,
            DateOnly.FromDateTime(DateTime.Now),
            [Receipt.Create("No comment", $"john.doe/receipt_{Guid.NewGuid()}.jpg")]);

        await Context.AddAsync(expected);
        _ = await ExpenseWriteRepository.UnitOfWork.SaveChangesAsync();

        var specification = new GetEmployeeExpenseByIdWithAllSpecification(employee.Id, expected.Id);

        // Act
        var results = await ExpenseReadRepository.GetAsync(specification);

        // Assert
        var actual = results.Single();
        actual.ShouldBe(expected);
        actual.Category.ShouldNotBeNull().Id.ShouldBe(expected.CategoryId);
        actual.Currency.ShouldNotBeNull().Id.ShouldBe(expected.CurrencyId);
        actual.Employee.ShouldNotBeNull().Id.ShouldBe(expected.EmployeeId.ShouldNotBeNull());

        // Cleanup
        _ = Context.Expenses.Remove(expected);
        _ = await ExpenseWriteRepository.UnitOfWork.SaveChangesAsync();
    }
}
