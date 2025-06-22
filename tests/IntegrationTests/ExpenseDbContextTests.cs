using Assessments.ExpenseManagement.Infrastructure.Presets;
using Assessments.ExpenseManagement.IntegrationTests.Abstractions;
using Assessments.ExpenseManagement.IntegrationTests.Fixtures;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace Assessments.ExpenseManagement.IntegrationTests;

public sealed class ExpenseDbContextTests(TestContainerFactory factory)
    : TestBase(factory), IClassFixture<TestContainerFactory>
{
    [Fact]
    public async Task Migrate_Should_EnsurePredefinedCategories()
    {
        // Arrange
        var expected = CategoryPresets.PredefinedCategories;

        // Act
        await Context.Database.MigrateAsync();

        // Assert
        var actual = await Context.Categories.AsNoTracking().ToListAsync();
        actual.ShouldBe(expected, true);
    }

    [Fact]
    public async Task Migrate_Should_EnsurePredefinedCurrencies()
    {
        // Arrange
        var expected = CurrencyPresets.PredefinedCurrencies;

        // Act
        await Context.Database.MigrateAsync();

        // Assert
        var actual = await Context.Currencies.AsNoTracking().ToListAsync();
        actual.ShouldBe(expected, true);
    }

    [Fact]
    public async Task Migrate_Should_EnsurePredefinedEmployees()
    {
        // Arrange
        var expected = EmployeePresets.PredefinedEmployees;

        // Act
        await Context.Database.MigrateAsync();

        // Assert
        var actual = await Context.Employees.AsNoTracking().ToListAsync();
        actual.ShouldBe(expected, true);
    }
}
