using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
