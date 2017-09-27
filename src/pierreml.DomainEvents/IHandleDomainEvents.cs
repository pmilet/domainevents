using System;

namespace pierreml.DomainEvents
{
    public interface IHandleDomainEvents<T>
    {
        Guid SubscriberId { get; }
        void HandleEvent(T domainEvent);
    }
}
