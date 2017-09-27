using pmilet.DomainEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiedraPapelTijeraApp
{
    public enum Jugada { None, Piedra, Papel, Tijera };

    public class Jugador 
    {
        IDomainEventBus _bus;
        string _nombre;
        public Jugador(string nombre, IDomainEventBus bus)
        {
            _nombre = nombre;
            _bus = bus;
        }

        public void Jugar( Jugada jugada )
        {
            _bus.Add<JugadaRealizada>(new JugadaRealizada( _nombre, jugada ));
        }

        public void Confirmar()
        {
            _bus.Commit<JugadaRealizada>();
        }
    }
}
