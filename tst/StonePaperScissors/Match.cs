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
    public class Match : HandleDomainEventsBase<PlayMade>, 
        IHandleDomainEvents<InvalidPlay>
    {
        public int player1Score = 0;
        public int player2Score = 0;
        public PlayType player1LastScore= PlayType.None;
        public PlayType player2LastScore = PlayType.None;
        readonly IDomainEventDispatcher domainEventDispatcher;

        public Match( IDomainEventDispatcher dispatcher):base( dispatcher)
        {
            domainEventDispatcher = dispatcher;
            dispatcher.Subscribe<InvalidPlay>(this);
        }

        public void End()
        {
                if( player1Score > player2Score )
                {
                    //publish an event notifying that the match ended and Player1 is the winner
                    domainEventDispatcher.Publish<MatchEnded>(new MatchEnded(PlayerType.Player1));
                }
                else 
                {
                    //publish an event notifying that the match ended and Player1 is the winner
                    domainEventDispatcher.Publish<MatchEnded>(new MatchEnded(PlayerType.Player2));
                }
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

        public override void HandleEvent(PlayMade domainEvent)
        {
            SavePlay(domainEvent);

            EvalOutcome();
        }

        public void HandleEvent(InvalidPlay domainEvent)
        {
            Console.WriteLine($"invalid play {domainEvent.Play.ToString()} made by {domainEvent.Player.ToString()}");
        }
    }
}