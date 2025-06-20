using Assessments.ExpenseManagement.Domain.Abstractions;
using Mediator;
using Shouldly;

namespace Assessments.ExpenseManagement.UnitTests.Domain;

public sealed class EntityTests
{
    [Fact]
    public void IsTransient_ShouldBeFalse_WhenIdIsNotEmpty()
    {
        // Arrange
        var entity = TestEntity.Create(Guid.NewGuid());

        // Act
        var result = entity.IsTransient;

        // Assert
        result.ShouldBeFalse();
    }

    [Fact]
    public void IsTransient_ShouldBeTrue_WhenIdIsEmpty()
    {
        // Arrange
        var entity = TestEntity.Create(Guid.Empty);

        // Act
        var result = entity.IsTransient;

        // Assert
        result.ShouldBeTrue();
    }

    private sealed class TestDomainEvent : INotification;

    private sealed class TestEntity : Entity
    {
        private TestEntity() { }
        public static TestEntity Create(Guid id) => new() { Id = id };
        public static TestEntity Create(bool isTransient) => Create(isTransient ? Guid.Empty : Guid.NewGuid());
    }

    #region Domain Events

    [Fact]
    public void AddEvent_Should_AddEventToDomainEvents()
    {
        // Arrange
        var entity = TestEntity.Create(Guid.NewGuid());
        var domainEvent = new TestDomainEvent();

        // Act
        entity.AddDomainEvent(domainEvent);

        // Assert
        entity.DomainEvents.ShouldContain(domainEvent);
    }

    [Fact]
    public void ClearEvent_Should_ClearDomainEvents()
    {
        // Arrange
        var entity = TestEntity.Create(Guid.NewGuid());
        var domainEvent = new TestDomainEvent();
        entity.AddDomainEvent(domainEvent);

        // Act
        entity.ClearDomainEvents();

        // Assert
        entity.DomainEvents.ShouldBeEmpty();
    }

    [Fact]
    public void RemoveEvent_Should_RemoveEventFromDomainEvents()
    {
        // Arrange
        var entity = TestEntity.Create(Guid.NewGuid());
        var domainEvent = new TestDomainEvent();
        entity.AddDomainEvent(domainEvent);

        // Act
        entity.RemoveDomainEvent(domainEvent);

        // Assert
        entity.DomainEvents.ShouldNotContain(domainEvent);
    }

    #endregion

    #region Equality and Hashing

    [Fact]
    public void Equals_ShouldReturnFalse_WhenAnyEntityIsTransient()
    {
        // Arrange
        var entity1 = TestEntity.Create(true);
        var entity2 = TestEntity.Create(true);

        // Act
        var result = entity1.Equals(entity2);

        // Assert
        result.ShouldBeFalse();
    }

    [Fact]
    public void Equals_ShouldReturnTrue_WhenBothEntitiesHaveSameNonEmptyId()
    {
        // Arrange
        var id = Guid.NewGuid();
        var entity1 = TestEntity.Create(id);
        var entity2 = TestEntity.Create(id);

        // Act
        var result = entity1.Equals(entity2);

        // Assert
        result.ShouldBeTrue();
    }

    #endregion
}
