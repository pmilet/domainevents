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
        public JugadaRealizada(JugadorType jugador, Jugada jugada)
            : base( "JugadaRealizada", "1.0")
        {
            Jugador = jugador;
            Jugada = jugada;
        }

        public JugadorType Jugador { get; private set; }
        public Jugada Jugada { get; private set; }

    }
}
