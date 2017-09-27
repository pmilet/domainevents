using System;
using System.Collections.Generic;
using System.Linq;

namespace pierreml.DomainEvents
{
    public class DomainEventBus : IDomainEventBus
    {
        private List<IDomainEvent> _events = new List<IDomainEvent>();
        private Dictionary<Type,object> _subscribers;
        private bool _publishing;

        public DomainEventBus()
        {
            this._publishing = false;
        }

        private Dictionary<Type,object> Subscribers
        {
            get
            {
                if (this._subscribers == null)
                {
                    this._subscribers = new Dictionary<Type,object>();
                }

                return this._subscribers;
            }
            set
            {
                this._subscribers = value;
            }
        }

        public void Add<T>(T domainEvent) where T : IDomainEvent
        {
            _events.Add(domainEvent);
        }

        public void Commit<T>() where T : IDomainEvent
        {
            foreach (IDomainEvent domainEvent in _events)
            {
                if (domainEvent.GetType() == typeof(T))
                {
                    T typedEvent = (T)domainEvent;
                    Publish(typedEvent);
                }
            }
            _events.Clear();
        }

        public void Publish<T>(T domainEvent) where T : IDomainEvent
        {
            if (!this._publishing && this.HasSubscribers())
            {
                try
                {
                    this._publishing = true;

                    foreach (var subscriber in this.Subscribers.Where( s=> s.Key == typeof(T) || s.Key == typeof(IDomainEvent)).Select(c=> c.Value))
                    {
                        if (subscriber is IHandleDomainEvents<T>)
                        {
                            IHandleDomainEvents<T> subscriberOfT = subscriber as IHandleDomainEvents<T>;
                            subscriberOfT.HandleEvent(domainEvent);
                        }
                        else
                        {
                            IHandleDomainEvents<IDomainEvent> subscriberOfT = subscriber as IHandleDomainEvents<IDomainEvent>;
                            subscriberOfT.HandleEvent(domainEvent);
                        }
                    }
                }
                finally
                {
                    this._publishing = false;
                }
            }
        }

        public void Reset()
        {
            if (!this._publishing)
            {
                this.Subscribers = null;
            }
        }

        public void Subscribe<T>(IHandleDomainEvents<T> subscriber) where T : IDomainEvent
        {
            if (!this._publishing && !Registered<T>(subscriber))
            {
                this.Subscribers.Add(typeof(T), subscriber as IHandleDomainEvents<T>);
            }
        }

        private bool Registered<T>(IHandleDomainEvents<T> subscriber)
        {
            return this.Subscribers.Where(s => s.Key == typeof(T) ).Any();
        }

        bool HasSubscribers()
        {
            return this._subscribers != null && this.Subscribers.Count != 0;
        }
    }
}
