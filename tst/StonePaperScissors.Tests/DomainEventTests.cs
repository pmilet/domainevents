// Copyright (c) 2017 Pierre Milet. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
using System;
using pmilet.DomainEvents;
using Xunit;

namespace StonePaperScissorsApp.Tests
{
    /// <summary>
    /// Tests for the DomainEvent base class.
    /// </summary>
    public class DomainEventTests
    {
        private class TestEvent : DomainEvent
        {
            public TestEvent(string source, string version = "1.0", string? subscriberId = null)
                : base(source, version, subscriberId) { }
        }

        [Fact]
        public void DomainEvent_SetsAggregateSource()
        {
            var evt = new TestEvent("MySource");
            Assert.Equal("MySource", evt.AggregateSource);
        }

        [Fact]
        public void DomainEvent_SetsVersion()
        {
            var evt = new TestEvent("MySource", "2.0");
            Assert.Equal("2.0", evt.Version);
        }

        [Fact]
        public void DomainEvent_DefaultVersion_IsOnePointZero()
        {
            var evt = new TestEvent("MySource");
            Assert.Equal("1.0", evt.Version);
        }

        [Fact]
        public void DomainEvent_SetsSubscriberId()
        {
            var evt = new TestEvent("MySource", "1.0", "sub-42");
            Assert.Equal("sub-42", evt.SubscriberId);
        }

        [Fact]
        public void DomainEvent_DefaultSubscriberId_IsNull()
        {
            var evt = new TestEvent("MySource");
            Assert.Null(evt.SubscriberId);
        }

        [Fact]
        public void DomainEvent_CreatedOn_IsApproximatelyNow()
        {
            var before = DateTime.UtcNow.AddSeconds(-1);
            var evt = new TestEvent("MySource");
            var after = DateTime.UtcNow.AddSeconds(1);

            Assert.True(evt.CreatedOn >= before && evt.CreatedOn <= after);
        }

        [Fact]
        public void DomainEvent_AggregateSource_CanBeChanged()
        {
            var evt = new TestEvent("OriginalSource");
            evt.AggregateSource = "NewSource";
            Assert.Equal("NewSource", evt.AggregateSource);
        }

        [Fact]
        public void DomainEvent_Version_CanBeChanged()
        {
            var evt = new TestEvent("MySource", "1.0");
            evt.Version = "3.0";
            Assert.Equal("3.0", evt.Version);
        }
    }
}
