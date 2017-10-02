using pmilet.DomainEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StonePaperScissorsApp
{
    public enum PlayType { None, Stone, Paper, Scissors };
    public enum PlayerType { None, Player1, Player2 }

    public abstract class Player 
    {
        IDomainEventBus _bus;
        PlayerType _player;
        public Player(PlayerType player, IDomainEventBus bus)
        {
            _player = player;
            _bus = bus;
        }

        public void Jugar( PlayType play )
        {
            _bus.Add<PlayMade>(new PlayMade( _player, play ));
        }

        public void Confirm()
        {
            _bus.Commit<PlayMade>();
        }
    }

    public class Player1 : Player
    {
        public Player1(IDomainEventBus bus) : base(PlayerType.Player1, bus)
        { }
    }

    public class Player2 : Player
    {
        public Player2(IDomainEventBus bus) : base(PlayerType.Player2, bus)
        { }
    }
}
