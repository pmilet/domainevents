using pmilet.DomainEvents;
using System;

namespace PiedraPapelTijeraApp
{
    class Program
    {
        static void Main(string[] args)
        {
            IDomainEventBus bus = new DomainEventBus();

            Jugador j1 = new Jugador1(bus);
            Jugador j2 = new Jugador2(bus);
            Partida partida = new Partida(bus);
            Resultados resultados = new Resultados(bus);

            bool exit = false;
            while( !exit )
            {
                Console.WriteLine("Jugador1: Piedra(1) Papel(2) o Tijera(3)");
                var s = Console.ReadLine();
                Jugada jugada = JugadaSeleccionada(s);
                j1.Jugar(jugada);
                j1.Confirmar();

                Console.WriteLine("Jugador2: Piedra(1) Papel(2) o Tijera(3)");
                s = Console.ReadLine();
                jugada = JugadaSeleccionada(s);
                j2.Jugar(jugada);
                j2.Confirmar();

                Console.WriteLine("exit? y/n");
                var r = Console.ReadLine();
                exit = r == "y";
            }

            partida.FinalizarPartida();
            Console.WriteLine($"Ganador {resultados.UltimoGanador()}");
            Console.ReadLine();
        }

        private static Jugada JugadaSeleccionada(string s)
        {
            var jugada = Jugada.None;
            switch (s)
            {
                case "1":
                    jugada = Jugada.Piedra;
                    break;
                case "2":
                    jugada = Jugada.Papel;
                    break;
                case "3":
                    jugada = Jugada.Tijera;
                    break;
                default:
                    jugada = Jugada.None;
                    break;
            }

            return jugada;
        }
    }
}
