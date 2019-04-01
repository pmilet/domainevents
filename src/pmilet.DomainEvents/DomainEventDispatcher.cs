// Copyright (c) 2017 Pierre Milet. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace pmilet.DomainEvents
{
    internal struct Subscriber
    {
        public string SubscriberId { get; set; }
        public Type SubscriberType { get; set; }
    }

    public class DomainEventDispatcher : IDomainEventDispatcher
    {
        private ConcurrentBag<IDomainEvent> _events = new ConcurrentBag<IDomainEvent>();
        private ConcurrentDictionary<Subscriber, object> _subscribers;
        private bool _publishing;

        public DomainEventDispatcher()
        {
            _publishing = false;
        }

        private ConcurrentDictionary<Subscriber, object> Subscribers
        {
            get
            {
                if (_subscribers == null)
                {
                    _subscribers = new ConcurrentDictionary<Subscriber, object>();
                }

                return _subscribers;
            }
            set => _subscribers = value;
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
            ConcurrentBag<IDomainEvent> newBag = new ConcurrentBag<IDomainEvent>();
            Interlocked.Exchange<ConcurrentBag<IDomainEvent>>(ref _events, newBag);
        }

        public void Publish<T>(T domainEvent) where T : IDomainEvent
        {
            if (HasSubscribers() && domainEvent != null)
            {
                try
                {
                    _publishing = true;
                    Type domainEventType = domainEvent.GetType();
                    IEnumerable<KeyValuePair<Subscriber, object>> suscribersToThisEvent = Subscribers.Where(s =>
                            (domainEventType == s.Key.SubscriberType ||
                            domainEventType.IsSubclassOf(s.Key.SubscriberType) ||
                            (s.Key.SubscriberType.IsInterface && domainEventType.GetInterfaces().Contains(s.Key.SubscriberType))) &&
                        (domainEvent.SubscriberId == null || domainEvent.SubscriberId == s.Key.SubscriberId));
                    foreach (KeyValuePair<Subscriber, object> subscriber in suscribersToThisEvent)
                    {
                        if (subscriber.Value is IHandleDomainEvents<T> subscriberOfT)
                        {
                            subscriberOfT.HandleEvent(domainEvent);
                        }
                        else if (subscriber.Value is HandleDomainEventsBase<T> subscriberOfBaseT)
                        {
                            subscriberOfBaseT.HandleDomainEvent(domainEvent);
                        }
                        else if (subscriber.Value is IHandleDomainEventsBase subscriberOfBase)
                        {
                            subscriberOfBase.HandleDomainEvent(domainEvent);
                        }
                    }
                }
                finally
                {
                    DomainEventSource.Current.Log(domainEvent);
                    _publishing = false;
                }
            }
        }

        private bool IsDomainEvent(Type domainEventType)
        {
            return typeof(DomainEvent).IsSubclassOf(domainEventType);
        }

        public void Reset()
        {
            if (!_publishing)
            {
                Subscribers = null;
            }
        }

        public void Subscribe<T>(IHandleDomainEvents<T> subscriber) where T : IDomainEvent
        {
            if (!_publishing && !Registered<T>(subscriber))
            {
                Subscribers.GetOrAdd(new Subscriber { SubscriberType = typeof(T), SubscriberId = subscriber.SubscriberId }, subscriber as IHandleDomainEvents<T>);
            }
        }

        private bool Registered<T>(IHandleDomainEvents<T> subscriber)
        {
            return Subscribers.Where(s => s.Key.SubscriberType == typeof(T) && subscriber.SubscriberId == s.Key.SubscriberId).Any();
        }

        private bool HasSubscribers()
        {
            return _subscribers != null && Subscribers.Count != 0;
        }
    }
}
