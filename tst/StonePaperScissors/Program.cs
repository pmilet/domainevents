﻿// Copyright (c) 2017 Pierre Milet. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
using pmilet.DomainEvents;
using System;

namespace StonePaperScissorsApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // we create the domain event dispatcher and inject it into the objects of our domain model (normally done using a IoC container) 
            IDomainEventDispatcher dispatcher = new DomainEventDispatcher();
            Player j1 = new Player1(dispatcher);
            Player j2 = new Player2(dispatcher);
            Match match = new Match(dispatcher);
            Outcome outcome = new Outcome(dispatcher);

            bool exit = false;
            while( !exit )
            {
                PlayType jugada = PlayType.None;
                do
                {
                    Console.WriteLine("Jugador1: Piedra(1) Papel(2) o Tijera(3)");
                    var s = Console.ReadLine();
                    jugada = JugadaSeleccionada(s);
                    j1.Play(jugada);
                    j1.Confirm();
                } while (jugada == PlayType.None);
                do {
                    Console.WriteLine("Jugador2: Piedra(1) Papel(2) o Tijera(3)");
                    var s = Console.ReadLine();
                    jugada = JugadaSeleccionada(s);
                    j2.Play(jugada);
                    j2.Confirm();
                } while (jugada == PlayType.None);
                Console.WriteLine("exit? y/n");
                var r = Console.ReadLine();
                exit = r == "y";
            }

            match.End();
            Console.WriteLine($"Ganador {outcome.LastWinner()}");
            Console.ReadLine();
        }

        private static PlayType JugadaSeleccionada(string s)
        {
            var jugada = PlayType.None;
            switch (s)
            {
                case "1":
                    jugada = PlayType.Stone;
                    break;
                case "2":
                    jugada = PlayType.Paper;
                    break;
                case "3":
                    jugada = PlayType.Scissors;
                    break;
                default:
                    jugada = PlayType.None;
                    break;
            }

            return jugada;
        }
    }
}
