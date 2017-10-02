using pmilet.DomainEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StonePaperScissorsApp
{
    /// <summary>
    /// Represents the play choosen by a player
    /// </summary>
    public class PlayMade : DomainEvent
    {
        public PlayMade(PlayerType player, PlayType play)
            : base( "PlayMade", "1.0")
        {
            Player = player;
            Play = play;
        }

        public PlayerType Player { get; private set; }
        public PlayType Play { get; private set; }

    }
}
