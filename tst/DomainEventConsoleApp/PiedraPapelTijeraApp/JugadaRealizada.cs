using pmilet.DomainEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiedraPapelTijeraApp
{
    public class JugadaRealizada : DomainEvent
    {
        public JugadaRealizada(string nombre, Jugada jugada): base( "JugadaRealizada", "1.0")
        {
            Jugador = nombre;
            Jugada = jugada;
        }

        public string Jugador { get; private set; }
        public Jugada Jugada { get; private set; }

    }
}
