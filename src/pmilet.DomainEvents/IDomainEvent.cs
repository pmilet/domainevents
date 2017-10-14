// Copyright (c) 2017 Pierre Milet. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
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
