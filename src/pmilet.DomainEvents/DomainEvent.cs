// Copyright (c) Pierre Milet. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
using System;

namespace pmilet.DomainEvents
{
    public class DomainEvent : IDomainEvent
    {
        public DomainEvent(string source, string version)
        {
            AggregateSource = source;
            Version = version;
            CreatedOn = DateTime.Now;
        }

        public string AggregateSource
        {
            get; private set;
        }

        public DateTime CreatedOn
        {
            get; private set;
        }

        public string Version
        {
            get; private set;
        }
    }
}
