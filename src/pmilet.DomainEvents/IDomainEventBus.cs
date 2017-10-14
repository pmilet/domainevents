﻿// Copyright (c) 2017 Pierre Milet. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pmilet.DomainEvents
{
    public interface IDomainEventBus
    {
        void Add<T>(T domainEvent) where T : IDomainEvent;
        void Commit<T>() where T : IDomainEvent;
        void Publish<T>(T domainEvent) where T : IDomainEvent;
        void Subscribe<T>(IHandleDomainEvents<T> subscriber) where T : IDomainEvent;
    }
}
