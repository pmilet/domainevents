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

        public void Play( PlayType play )
        {
            //delayed event to notify of the move choosen by the player
            _bus.Add<PlayMade>(new PlayMade( _player, play ));
        }

        public void Confirm()
        {
            //commit all registered delayed events
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
