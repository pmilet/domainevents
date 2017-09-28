using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiedraPapelTijeraApp
{
    public class PartidaFinalizada : pmilet.DomainEvents.DomainEvent
    {
        public PartidaFinalizada(JugadorType jugador)
            : base("PartidaFinalizada", "1.0")
        {
            Ganador = jugador;
        }

        public JugadorType Ganador { get; private set; }
    }
}
