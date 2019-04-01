// Copyright (c) 2017 Pierre Milet. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
using System;

namespace pmilet.DomainEvents
{
    public class DomainEvent : IDomainEvent
    {
        public DomainEvent(string source, string version = "1.0", string subscriberId = null)
        {
            AggregateSource = source;
            Version = version;
            CreatedOn = DateTime.UtcNow;
            SubscriberId = subscriberId;
        }

        public string AggregateSource { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Version { get; set; }

        public string SubscriberId { get; }
    }
}
