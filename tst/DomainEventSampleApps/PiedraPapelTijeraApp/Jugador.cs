using pmilet.DomainEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiedraPapelTijeraApp
{
    public enum Jugada { None, Piedra, Papel, Tijera };
    public enum JugadorType { None,Jugador1, Jugador2 }

    public abstract class Jugador 
    {
        IDomainEventBus _bus;
        JugadorType _jugador;
        public Jugador(JugadorType jugador, IDomainEventBus bus)
        {
            _jugador = jugador;
            _bus = bus;
        }

        public void Jugar( Jugada jugada )
        {
            _bus.Add<JugadaRealizada>(new JugadaRealizada( _jugador, jugada ));
        }

        public void Confirmar()
        {
            _bus.Commit<JugadaRealizada>();
        }
    }

    public class Jugador1 : Jugador
    {
        public Jugador1(IDomainEventBus bus) : base(JugadorType.Jugador1, bus)
        { }
    }

    public class Jugador2 : Jugador
    {
        public Jugador2(IDomainEventBus bus) : base(JugadorType.Jugador2, bus)
        { }
    }
}
