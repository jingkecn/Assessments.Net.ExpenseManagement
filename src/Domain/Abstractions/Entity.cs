using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Mediator;

namespace Assessments.ExpenseManagement.Domain.Abstractions;

public abstract class Entity
{
    private readonly List<INotification> domainEvents = [];

    [JsonIgnore] [NotMapped] public IReadOnlyCollection<INotification> DomainEvents => domainEvents.AsReadOnly();

    [Required] public Guid Id { get; protected init; }

    [JsonIgnore] [NotMapped] public bool IsTransient => Id == Guid.Empty;

    #region Domain Events

    public void AddDomainEvent(INotification eventItem) => domainEvents.Add(eventItem);

    public void ClearDomainEvents() => domainEvents.Clear();

    public void RemoveDomainEvent(INotification eventItem) => domainEvents.Remove(eventItem);

    #endregion

    #region Equality and Hashing

    public override bool Equals(object? obj) => obj is Entity other && Equals(other);

    private bool Equals(Entity other)
    {
        if (ReferenceEquals(this, other))
        {
            return true;
        }

        if (IsTransient || other.IsTransient)
        {
            return false;
        }

        return Id.Equals(other.Id);
    }

    public override int GetHashCode() => IsTransient ? base.GetHashCode() : Id.GetHashCode() ^ 31;

    #endregion
}
