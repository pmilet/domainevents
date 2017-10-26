// Copyright (c) 2017 Pierre Milet. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
using pmilet.DomainEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StonePaperScissorsApp
{
    public class Outcome : IHandleDomainEvents<MatchEnded>
    {
        PlayerType _lastWinner;
        private readonly IDomainEventDispatcher _dispatcher;
        public Outcome(IDomainEventDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
            dispatcher.Subscribe<MatchEnded>(this);
        }

        public Guid SubscriberId => throw new NotImplementedException();

        public void HandleEvent(MatchEnded domainEvent)
        {
            _lastWinner = domainEvent.Winner;
        }

        public PlayerType LastWinner()
        {
            return _lastWinner;
        }

    }
}
