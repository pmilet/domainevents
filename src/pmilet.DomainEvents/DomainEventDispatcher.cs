// Copyright (c) 2017 Pierre Milet. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace pmilet.DomainEvents
{
    public class DomainEventDispatcher : IDomainEventDispatcher
    {
        private ConcurrentBag<IDomainEvent> _events = new ConcurrentBag<IDomainEvent>();
        private ConcurrentDictionary<Type, object> _subscribers;
        private bool _publishing;

        public DomainEventDispatcher()
        {
            this._publishing = false;
        }

        private ConcurrentDictionary<Type, object> Subscribers
        {
            get
            {
                if (this._subscribers == null)
                {
                    this._subscribers = new ConcurrentDictionary<Type, object>();
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
            var newBag = new ConcurrentBag<IDomainEvent>();
            Interlocked.Exchange<ConcurrentBag<IDomainEvent>>(ref _events, newBag);
        }

        public void Publish<T>(T domainEvent) where T : IDomainEvent
        {
            if (this.HasSubscribers() && domainEvent != null)
            {
                try
                {
                    this._publishing = true;
                    Type domainEventType = domainEvent.GetType();

                    var suscribersToThisEvent = this.Subscribers.Where(s => domainEventType == s.Key || domainEventType.IsSubclassOf(s.Key));
                    foreach (var subscriber in suscribersToThisEvent)
                    {
                        if (subscriber.Value is HandleDomainEventsBase<T> subscriberOfBase)
                            subscriberOfBase.HandleDomainEvent(domainEvent);
                        if (subscriber.Value is IHandleDomainEvents<T> subscriberOfT)
                            subscriberOfT.HandleEvent(domainEvent);
                    }
                }
                finally
                {
                    DomainEventSource.Current.Log(domainEvent);
                    this._publishing = false;
                }
            }
        }


        private bool IsDomainEvent(Type domainEventType)
        {
            return typeof(DomainEvent).IsSubclassOf(domainEventType);
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
                this.Subscribers.GetOrAdd(typeof(T), subscriber as IHandleDomainEvents<T>);
            }
        }

        private bool Registered<T>(IHandleDomainEvents<T> subscriber)
        {
            return this.Subscribers.Where(s => s.Key == typeof(T)).Any();
        }

        bool HasSubscribers()
        {
            return this._subscribers != null && this.Subscribers.Count != 0;
        }
    }
}
