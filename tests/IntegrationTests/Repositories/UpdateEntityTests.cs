using Assessments.ExpenseManagement.Domain.Models;
using Assessments.ExpenseManagement.Infrastructure.Presets;
using Assessments.ExpenseManagement.IntegrationTests.Abstractions;
using Assessments.ExpenseManagement.IntegrationTests.Fixtures;
using Shouldly;

namespace Assessments.ExpenseManagement.IntegrationTests.Repositories;

[Collection(nameof(TestCollectionFixture))]
public sealed class UpdateEntityTests(TestContainerFactory factory) : TestBaseWithDbMigration(factory)
{
    [Fact]
    public async Task Should_UpdateEntity_When_Successful()
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

        expected.Submit(); // Change status to 'Submitted'

        // Act
        var actual = await ExpenseWriteRepository.UpdateAsync(expected);
        _ = await ExpenseWriteRepository.UnitOfWork.SaveChangesAsync();

        // Assert
        actual.ShouldBeTrue();
        Context.Expenses.ShouldContain(expected);
        Context.Receipts.ShouldContain(receipt);

        // Cleanup
        _ = Context.Expenses.Remove(expected);
        _ = await ExpenseWriteRepository.UnitOfWork.SaveChangesAsync();
    }
}
