using pmilet.DomainEvents;
using System;

namespace PiedraPapelTijeraApp
{
    class Program
    {
        static void Main(string[] args)
        {
            IDomainEventBus bus = new DomainEventBus();

            Console.WriteLine("jugador1 : ");
            var nombre = Console.ReadLine();
            Jugador j1 = new Jugador(nombre, bus);
            Console.WriteLine("jugador2 : ");
            nombre = Console.ReadLine();
            Jugador j2 = new Jugador(nombre, bus);
            Partida partida = new Partida(bus);

            bool exit = false;
            while( !exit )
            {
                Console.WriteLine("Jugador1: Piedra(1) Papel(2) o Tijera(3)");
                var s = Console.ReadLine();
                Jugada jugada = JugadaSeleccionada(s);
                j1.Jugar(jugada);
                j1.Confirmar();

                Console.WriteLine("Jugador1: Piedra(1) Papel(2) o Tijera(3)");
                s = Console.ReadLine();
                jugada = JugadaSeleccionada(s);
                j2.Jugar(jugada);
                j2.Confirmar();

                Console.WriteLine("exit? y/n");
                var r = Console.ReadLine();
                exit = r == "y";
            }

            Console.WriteLine($"{partida.Resultado}");
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
