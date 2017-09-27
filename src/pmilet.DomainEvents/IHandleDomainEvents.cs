using System;

namespace pmilet.DomainEvents
{
    public interface IHandleDomainEvents<T>
    {
        Guid SubscriberId { get; }
        void HandleEvent(T domainEvent);
    }
}
