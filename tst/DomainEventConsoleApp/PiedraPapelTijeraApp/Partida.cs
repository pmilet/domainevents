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
        public string jugador1;
        public string jugador2;
        public Jugada ultimaJugadaJugador1 = Jugada.None;
        public Jugada ultimaJugadaJugador2 = Jugada.None;

        public Partida( IDomainEventBus bus)
        {
            bus.Subscribe<JugadaRealizada>(this);

        }

        public string Resultado {
            get
            {
                if( puntuacionJugador1 > puntuacionJugador2 )
                {
                    return $" el ganador es {jugador1} por {puntuacionJugador1} a {puntuacionJugador2}";
                }
                else if(puntuacionJugador2 > puntuacionJugador1)
                {
                    return $" el ganador es {jugador2} por {puntuacionJugador2} a {puntuacionJugador1}";
                }
                else
                {
                    return $"empate a {puntuacionJugador2}";
                }
            }
        }

        public Guid SubscriberId => Guid.NewGuid();

        public void HandleEvent(JugadaRealizada domainEvent)
        {
            DeterminarNombresJugadores(domainEvent);

            DeterminarQuienJuega(domainEvent);

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

        private void DeterminarQuienJuega(JugadaRealizada domainEvent)
        {
            if (domainEvent.Jugador == jugador1)
            {
                ultimaJugadaJugador1 = domainEvent.Jugada;
            }
            else if (domainEvent.Jugador == jugador2)
            {
                ultimaJugadaJugador2 = domainEvent.Jugada;
            }
        }

        private void DeterminarNombresJugadores(JugadaRealizada domainEvent)
        {
        }
    }
}