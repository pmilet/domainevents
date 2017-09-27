using System;

namespace pmilet.DomainEvents
{
    public interface IDomainEvent
    {
        string AggregateSource { get; }
        DateTime CreatedOn { get; }
        string Version { get; }
    }
}
