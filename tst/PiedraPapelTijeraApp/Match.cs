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

        IDomainEventBus _bus;
        public Match( IDomainEventBus bus)
        {
            _bus = bus;
            bus.Subscribe<PlayMade>(this);

        }

        public void End()
        {
                if( player1Score > player2Score )
                {
                    _bus.Publish<MatchEnded>(new MatchEnded(PlayerType.Player1));
                }
                else if(player2Score > player1Score)
                {
                    _bus.Publish<MatchEnded>(new MatchEnded(PlayerType.Player2));
                }
                else
                {
                    _bus.Publish<MatchEnded>(new MatchEnded(PlayerType.None));
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