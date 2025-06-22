using Assessments.ExpenseManagement.Domain.Models;
using Assessments.ExpenseManagement.Infrastructure.Presets;
using Assessments.ExpenseManagement.IntegrationTests.Abstractions;
using Assessments.ExpenseManagement.IntegrationTests.Fixtures;
using Shouldly;

namespace Assessments.ExpenseManagement.IntegrationTests.Repositories;

[Collection(nameof(TestCollectionFixture))]
public sealed class DeleteEntityTests(TestContainerFactory factory) : TestBaseWithDbMigration(factory)
{
    [Fact]
    public async Task Should_DeleteEntity_When_Successful()
    {
        // Arrange
        var employee = EmployeePresets.JohnDoe;
        var receipt = Receipt.Create("No comment", $"john.doe/receipt_{Guid.NewGuid()}.jpg");
        var expected = Expense.Create(
            employee,
            100M,
            CategoryPresets.Hotel,
            CurrencyPresets.USD,
            DateOnly.FromDateTime(DateTime.Now),
            [receipt]);

        await Context.Expenses.AddAsync(expected);
        _ = await ExpenseWriteRepository.UnitOfWork.SaveChangesAsync();

        // Act
        var actual = await ExpenseWriteRepository.DeleteAsync(expected);
        _ = await ExpenseWriteRepository.UnitOfWork.SaveChangesAsync();

        // Assert
        actual.ShouldBeTrue();
        Context.Expenses.ShouldNotContain(expected);
        Context.Receipts.ShouldNotContain(receipt);
    }
}
