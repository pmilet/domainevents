using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pierreml.DomainEvents
{
    public interface IDomainEventBus
    {
        void Add<T>(T domainEvent) where T : IDomainEvent;
        void Commit<T>() where T : IDomainEvent;
        void Publish<T>(T domainEvent) where T : IDomainEvent;
        void Subscribe<T>(IHandleDomainEvents<T> subscriber) where T : IDomainEvent;
    }
}
