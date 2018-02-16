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
        IDomainEventDispatcher _dispatcher;
        PlayerType _player;
        public Player(PlayerType player, IDomainEventDispatcher dispatcher)
        {
            _player = player;
            _dispatcher = dispatcher;
        }

        public void Play( PlayType play )
        {
            //delayed event to notify of the move choosen by the player
            _dispatcher.Add<PlayMade>(new PlayMade( _player, play ));
            if( play == PlayType.None)
                _dispatcher.Publish<InvalidPlay>(new InvalidPlay(_player, play));
        }

        public void Confirm()
        {
            //commit all registered delayed events
            _dispatcher.Commit<PlayMade>();
        }
    }

    public class Player1 : Player
    {
        public Player1(IDomainEventDispatcher dispatcher) : base(PlayerType.Player1, dispatcher)
        { }
    }

    public class Player2 : Player
    {
        public Player2(IDomainEventDispatcher dispatcher) : base(PlayerType.Player2, dispatcher)
        { }
    }
}
