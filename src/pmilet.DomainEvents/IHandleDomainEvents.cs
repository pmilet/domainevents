// Copyright (c) Pierre Milet. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
using System;

namespace pmilet.DomainEvents
{
    public interface IHandleDomainEvents<T>
    {
        Guid SubscriberId { get; }
        void HandleEvent(T domainEvent);
    }
}
