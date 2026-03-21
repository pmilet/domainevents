// Copyright (c) 2017 Pierre Milet. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
using System.Collections.Generic;
using pmilet.DomainEvents;
using Xunit;

namespace StonePaperScissorsApp.Tests
{
    /// <summary>
    /// Comprehensive tests for DomainEventDispatcher covering all public methods and edge cases.
    /// </summary>
    public class DomainEventDispatcherTests
    {
        // ---------------------------------------------------------------------------
        // Test helpers
        // ---------------------------------------------------------------------------

        private class OrderEvent : DomainEvent
        {
            public OrderEvent(string? subscriberId = null) : base("Order", "1.0", subscriberId) { }
        }

        private class ShipmentEvent : DomainEvent
        {
            public ShipmentEvent(string? subscriberId = null) : base("Shipment", "1.0", subscriberId) { }
        }

        private class RecordingHandler<T> : IHandleDomainEvents<T> where T : IDomainEvent
        {
            public List<T> ReceivedEvents { get; } = new List<T>();
            public string? SubscriberId { get; }

            public RecordingHandler(string? subscriberId = null)
            {
                SubscriberId = subscriberId;
            }

            public void HandleEvent(T domainEvent)
            {
                ReceivedEvents.Add(domainEvent);
            }
        }

        private class ConcreteHandlerBase<T> : HandleDomainEventsBase<T> where T : IDomainEvent
        {
            public List<T> ReceivedEvents { get; } = new List<T>();

            public ConcreteHandlerBase(IDomainEventDispatcher dispatcher) : base(dispatcher) { }
            public ConcreteHandlerBase(IDomainEventDispatcher dispatcher, string subscriberId)
                : base(dispatcher, subscriberId) { }

            public override void HandleEvent(T domainEvent)
            {
                ReceivedEvents.Add(domainEvent);
            }
        }

        // ---------------------------------------------------------------------------
        // Subscribe / Publish
        // ---------------------------------------------------------------------------

        [Fact]
        public void Publish_WithSubscriber_DeliversEvent()
        {
            var dispatcher = new DomainEventDispatcher();
            var handler = new RecordingHandler<OrderEvent>();
            dispatcher.Subscribe(handler);

            var evt = new OrderEvent();
            dispatcher.Publish(evt);

            Assert.Single(handler.ReceivedEvents);
            Assert.Same(evt, handler.ReceivedEvents[0]);
        }

        [Fact]
        public void Publish_WithNoSubscribers_DoesNotThrow()
        {
            var dispatcher = new DomainEventDispatcher();
            dispatcher.Publish(new OrderEvent()); // should not throw
        }

        [Fact]
        public void Publish_NullEvent_DoesNotThrow()
        {
            var dispatcher = new DomainEventDispatcher();
            var handler = new RecordingHandler<OrderEvent>();
            dispatcher.Subscribe(handler);

            dispatcher.Publish<OrderEvent>(null!); // should not throw

            Assert.Empty(handler.ReceivedEvents);
        }

        [Fact]
        public void Publish_WrongEventType_IsNotDeliveredToHandler()
        {
            var dispatcher = new DomainEventDispatcher();
            var handler = new RecordingHandler<OrderEvent>();
            dispatcher.Subscribe(handler);

            dispatcher.Publish(new ShipmentEvent());

            Assert.Empty(handler.ReceivedEvents);
        }

        [Fact]
        public void Publish_MultipleSubscribers_AllReceiveEvent()
        {
            var dispatcher = new DomainEventDispatcher();
            // Subscribers need distinct SubscriberIds to both be registered
            var h1 = new RecordingHandler<OrderEvent>("subscriber-1");
            var h2 = new RecordingHandler<OrderEvent>("subscriber-2");
            dispatcher.Subscribe(h1);
            dispatcher.Subscribe(h2);

            // Event with null SubscriberId reaches all subscribers
            dispatcher.Publish(new OrderEvent());

            Assert.Single(h1.ReceivedEvents);
            Assert.Single(h2.ReceivedEvents);
        }

        [Fact]
        public void Publish_WithSubscriberId_OnlyDeliversToMatchingSubscriber()
        {
            var dispatcher = new DomainEventDispatcher();
            var handlerA = new RecordingHandler<OrderEvent>("subscriberA");
            var handlerB = new RecordingHandler<OrderEvent>("subscriberB");
            dispatcher.Subscribe(handlerA);
            dispatcher.Subscribe(handlerB);

            // Event targeted at subscriberA
            dispatcher.Publish(new OrderEvent("subscriberA"));

            Assert.Single(handlerA.ReceivedEvents);
            Assert.Empty(handlerB.ReceivedEvents);
        }

        [Fact]
        public void Publish_EventWithNullSubscriberId_DeliversToAllSubscribers()
        {
            var dispatcher = new DomainEventDispatcher();
            var handlerA = new RecordingHandler<OrderEvent>("subscriberA");
            var handlerB = new RecordingHandler<OrderEvent>("subscriberB");
            dispatcher.Subscribe(handlerA);
            dispatcher.Subscribe(handlerB);

            // Event with no specific target
            dispatcher.Publish(new OrderEvent(null));

            Assert.Single(handlerA.ReceivedEvents);
            Assert.Single(handlerB.ReceivedEvents);
        }

        [Fact]
        public void Subscribe_SameSubscriberTwice_IsOnlyRegisteredOnce()
        {
            var dispatcher = new DomainEventDispatcher();
            var handler = new RecordingHandler<OrderEvent>();
            dispatcher.Subscribe(handler);
            dispatcher.Subscribe(handler); // duplicate

            dispatcher.Publish(new OrderEvent());

            Assert.Single(handler.ReceivedEvents);
        }

        // ---------------------------------------------------------------------------
        // Add / Commit
        // ---------------------------------------------------------------------------

        [Fact]
        public void Add_ThenCommit_DeliversEvent()
        {
            var dispatcher = new DomainEventDispatcher();
            var handler = new RecordingHandler<OrderEvent>();
            dispatcher.Subscribe(handler);

            dispatcher.Add(new OrderEvent());
            Assert.Empty(handler.ReceivedEvents); // not yet delivered

            dispatcher.Commit<OrderEvent>();
            Assert.Single(handler.ReceivedEvents);
        }

        [Fact]
        public void Commit_ClearsEventQueue()
        {
            var dispatcher = new DomainEventDispatcher();
            var handler = new RecordingHandler<OrderEvent>();
            dispatcher.Subscribe(handler);

            dispatcher.Add(new OrderEvent());
            dispatcher.Commit<OrderEvent>();
            dispatcher.Commit<OrderEvent>(); // second commit - nothing queued

            Assert.Single(handler.ReceivedEvents); // still only one delivery
        }

        [Fact]
        public void Commit_OnlyDeliversMatchingEventType()
        {
            var dispatcher = new DomainEventDispatcher();
            var orderHandler = new RecordingHandler<OrderEvent>();
            var shipHandler = new RecordingHandler<ShipmentEvent>();
            dispatcher.Subscribe(orderHandler);
            dispatcher.Subscribe(shipHandler);

            dispatcher.Add(new OrderEvent());
            dispatcher.Add(new ShipmentEvent());

            dispatcher.Commit<OrderEvent>(); // only OrderEvents should be dispatched

            Assert.Single(orderHandler.ReceivedEvents);
            Assert.Empty(shipHandler.ReceivedEvents);
        }

        [Fact]
        public void Add_MultipleEvents_ThenCommit_DeliversAll()
        {
            var dispatcher = new DomainEventDispatcher();
            var handler = new RecordingHandler<OrderEvent>();
            dispatcher.Subscribe(handler);

            dispatcher.Add(new OrderEvent());
            dispatcher.Add(new OrderEvent());
            dispatcher.Add(new OrderEvent());
            dispatcher.Commit<OrderEvent>();

            Assert.Equal(3, handler.ReceivedEvents.Count);
        }

        // ---------------------------------------------------------------------------
        // HandleDomainEventsBase
        // ---------------------------------------------------------------------------

        [Fact]
        public void HandleDomainEventsBase_AutoSubscribesOnConstruction()
        {
            var dispatcher = new DomainEventDispatcher();
            var handler = new ConcreteHandlerBase<OrderEvent>(dispatcher);

            dispatcher.Publish(new OrderEvent());

            Assert.Single(handler.ReceivedEvents);
        }

        [Fact]
        public void HandleDomainEventsBase_WithSubscriberId_SetsSubscriberId()
        {
            var dispatcher = new DomainEventDispatcher();
            var handler = new ConcreteHandlerBase<OrderEvent>(dispatcher, "sub-99");

            Assert.Equal("sub-99", handler.SubscriberId);
        }

        [Fact]
        public void HandleDomainEventsBase_HandleDomainEvent_InvokesHandleEvent()
        {
            var dispatcher = new DomainEventDispatcher();
            var handler = new ConcreteHandlerBase<OrderEvent>(dispatcher);
            var evt = new OrderEvent();

            handler.HandleDomainEvent(evt);

            Assert.Single(handler.ReceivedEvents);
            Assert.Same(evt, handler.ReceivedEvents[0]);
        }

        // ---------------------------------------------------------------------------
        // Reset
        // ---------------------------------------------------------------------------

        [Fact]
        public void Reset_ClearsAllSubscribers()
        {
            var dispatcher = new DomainEventDispatcher();
            var handler = new RecordingHandler<OrderEvent>();
            dispatcher.Subscribe(handler);

            dispatcher.Reset();
            dispatcher.Publish(new OrderEvent());

            Assert.Empty(handler.ReceivedEvents);
        }
    }
}
