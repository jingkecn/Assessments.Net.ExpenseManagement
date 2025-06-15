using Assessments.ExpenseManagement.Infrastructure.Models;
using Assessments.ExpenseManagement.IntegrationTests.Abstractions;
using Assessments.ExpenseManagement.IntegrationTests.Fixtures;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace Assessments.ExpenseManagement.IntegrationTests.Contexts;

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
        var actual = await Context.Categories.ToListAsync();
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
        var actual = await Context.Currencies.ToListAsync();
        actual.ShouldBe(expected, true);
    }

    [Fact]
    public async Task Migrate_Should_EnsurePredefinedUsers()
    {
        // Arrange
        var expected = UserPresets.PredefinedUsers;

        // Act
        await Context.Database.MigrateAsync();

        // Assert
        var actual = await Context.Users.ToListAsync();
        actual.ShouldBe(expected, true);
    }
}
