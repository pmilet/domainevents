// Copyright (c) 2017 Pierre Milet. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace StonePaperScissorsApp
{
    public class MatchEnded: pmilet.DomainEvents.DomainEvent
    {
        public MatchEnded(PlayerType player)
            : base("MatchEnds", "1.0")
        {
            Winner = player;
        }

        public PlayerType Winner { get; private set; }
    }
}
