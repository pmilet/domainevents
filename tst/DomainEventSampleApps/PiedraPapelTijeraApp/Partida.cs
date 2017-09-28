using pmilet.DomainEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiedraPapelTijeraApp
{
    public class Partida : IHandleDomainEvents<JugadaRealizada>
    {
        public int puntuacionJugador1 = 0;
        public int puntuacionJugador2 = 0;
        public Jugada ultimaJugadaJugador1 = Jugada.None;
        public Jugada ultimaJugadaJugador2 = Jugada.None;

        IDomainEventBus _bus;
        public Partida( IDomainEventBus bus)
        {
            _bus = bus;
            bus.Subscribe<JugadaRealizada>(this);

        }

        public void FinalizarPartida()
        {
                if( puntuacionJugador1 > puntuacionJugador2 )
                {
                    _bus.Publish<PartidaFinalizada>(new PartidaFinalizada(JugadorType.Jugador1));
                }
                else if(puntuacionJugador2 > puntuacionJugador1)
                {
                    _bus.Publish<PartidaFinalizada>(new PartidaFinalizada(JugadorType.Jugador2));
                }
                else
                {
                    _bus.Publish<PartidaFinalizada>(new PartidaFinalizada(JugadorType.None));
                }
        }

        public Guid SubscriberId => Guid.NewGuid();

        public void HandleEvent(JugadaRealizada domainEvent)
        {
            MemorizarJugada(domainEvent);

            EvaluarResultadoJugada();
        }
    
        private void EvaluarResultadoJugada()
        {
            if (ultimaJugadaJugador1 != Jugada.None & ultimaJugadaJugador2 != Jugada.None)
            {
                if (ultimaJugadaJugador1 == Jugada.Papel && ultimaJugadaJugador2 == Jugada.Piedra)
                {
                    puntuacionJugador1 += 1;
                    ultimaJugadaJugador1 = Jugada.None;
                }
                else if (ultimaJugadaJugador1 == Jugada.Piedra && ultimaJugadaJugador2 == Jugada.Papel)
                {
                    puntuacionJugador2 += 1;
                    ultimaJugadaJugador2 = Jugada.None;
                }
                else if (ultimaJugadaJugador1 == Jugada.Tijera && ultimaJugadaJugador2 == Jugada.Papel)
                {
                    puntuacionJugador1 += 1;
                    ultimaJugadaJugador1 = Jugada.None;
                }
                else if (ultimaJugadaJugador1 == Jugada.Papel && ultimaJugadaJugador2 == Jugada.Tijera)
                {
                    puntuacionJugador2 += 1;
                    ultimaJugadaJugador2 = Jugada.None;
                }
                else if (ultimaJugadaJugador1 == Jugada.Piedra && ultimaJugadaJugador2 == Jugada.Tijera)
                {
                    puntuacionJugador1 += 1;
                    ultimaJugadaJugador1 = Jugada.None;
                }
                else if (ultimaJugadaJugador1 == Jugada.Tijera && ultimaJugadaJugador2 == Jugada.Piedra)
                {
                    puntuacionJugador2 += 1;
                    ultimaJugadaJugador2 = Jugada.None;
                }
            }
        }

        private void MemorizarJugada(JugadaRealizada domainEvent)
        {
            if (domainEvent.Jugador ==  JugadorType.Jugador1)
            {
                ultimaJugadaJugador1 = domainEvent.Jugada;
            }
            else if (domainEvent.Jugador == JugadorType.Jugador2)
            {
                ultimaJugadaJugador2 = domainEvent.Jugada;
            }
        }

        
    }
}