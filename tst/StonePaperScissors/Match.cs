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
    public class Match : IHandleDomainEvents<PlayMade>
    {
        public int player1Score = 0;
        public int player2Score = 0;
        public PlayType player1LastScore= PlayType.None;
        public PlayType player2LastScore = PlayType.None;

        IDomainEventDispatcher _dispatcher;
        public Match( IDomainEventDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
            dispatcher.Subscribe<PlayMade>(this);

        }

        public void End()
        {
                if( player1Score > player2Score )
                {
                    //publish an event notifying that the match ended and Player1 is the winner
                    _dispatcher.Publish<MatchEnded>(new MatchEnded(PlayerType.Player1));
                }
                else 
                {
                    //publish an event notifying that the match ended and Player1 is the winner
                    _dispatcher.Publish<MatchEnded>(new MatchEnded(PlayerType.Player2));
                }
        }

        public Guid SubscriberId => Guid.NewGuid();

        public void HandleEvent(PlayMade domainEvent)
        {
            SavePlay(domainEvent);

            EvalOutcome();
        }
    
        private void EvalOutcome()
        {
            if (player1LastScore != PlayType.None & player2LastScore != PlayType.None)
            {
                if (player1LastScore == PlayType.Paper && player2LastScore == PlayType.Stone)
                {
                    player1Score += 1;
                    player1LastScore = PlayType.None;
                }
                else if (player1LastScore == PlayType.Stone && player2LastScore == PlayType.Paper)
                {
                    player2Score += 1;
                    player2LastScore = PlayType.None;
                }
                else if (player1LastScore == PlayType.Scissors && player2LastScore == PlayType.Paper)
                {
                    player1Score += 1;
                    player1LastScore = PlayType.None;
                }
                else if (player1LastScore == PlayType.Paper && player2LastScore == PlayType.Scissors)
                {
                    player2Score += 1;
                    player2LastScore = PlayType.None;
                }
                else if (player1LastScore == PlayType.Stone && player2LastScore == PlayType.Scissors)
                {
                    player1Score += 1;
                    player1LastScore = PlayType.None;
                }
                else if (player1LastScore == PlayType.Scissors && player2LastScore == PlayType.Stone)
                {
                    player2Score += 1;
                    player2LastScore = PlayType.None;
                }
            }
        }

        private void SavePlay(PlayMade domainEvent)
        {
            if (domainEvent.Player ==  PlayerType.Player1)
            {
                player1LastScore = domainEvent.Play;
            }
            else if (domainEvent.Player == PlayerType.Player2)
            {
                player2LastScore = domainEvent.Play;
            }
        }

        
    }
}