using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pmilet.DomainEvents
{
    public abstract class HandleDomainEventsBase<T> :
        IHandleDomainEvents<T>,
        IHandleDomainEventsBase where T : IDomainEvent
    {
        public string SubscriberId { get; }

        public HandleDomainEventsBase(IDomainEventDispatcher domainEventDispatcher, string subscriberId)
        {
            SubscriberId = subscriberId;
            domainEventDispatcher.Subscribe<T>(this);
        }

        protected HandleDomainEventsBase(IDomainEventDispatcher domainEventDispatcher)
        {
            domainEventDispatcher.Subscribe<T>(this);
        }

        public void HandleDomainEvent(IDomainEvent domainEvent)
        {
            HandleEvent((T)domainEvent);
        }

        public abstract void HandleEvent(T domainEvent);
    }
}
