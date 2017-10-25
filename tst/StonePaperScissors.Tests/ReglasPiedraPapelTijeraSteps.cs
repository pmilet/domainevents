using Microsoft.VisualStudio.TestTools.UnitTesting;
using pmilet.DomainEvents;
using System;
using TechTalk.SpecFlow;

namespace PiedraPapelTijeraApp.Tests
{
    [Binding]
    public class ReglasPiedraPapelTijeraSteps
    {
        private readonly IDomainEventBus bus;
        private readonly Partida partida;
        private readonly Jugador1 jugador1;
        private readonly Jugador2 jugador2;
        private readonly Resultados resultados;
        public ReglasPiedraPapelTijeraSteps()
        {
            bus = new DomainEventBus();
            partida = new Partida(bus);
            jugador1 = new Jugador1(bus);
            jugador2 = new Jugador2(bus);
            resultados = new Resultados(bus);
        }

        [Given(@"Jugador1 juega Piedra")]
        public void GivenJugador1JuegaPiedra()
        {
            jugador1.Jugar(Jugada.Piedra);
            jugador1.Confirmar();
        }
        
        [Given(@"y Jugador2 juega Tijera")]
        public void GivenYJugador2JuegaTijera()
        {
            jugador2.Jugar(Jugada.Tijera);
            jugador2.Confirmar();
        }
        
        [When(@"Finalizo la partida")]
        public void WhenFinalizoLaPartida()
        {
            partida.FinalizarPartida();
        }
        
        [Then(@"El ganador es Jugador(.*)")]
        public void ThenElGanadorEsJugador(int p0)
        {
            JugadorType ganadorEsperado = JugadorType.None;
            switch( p0)
            {
                case 1: ganadorEsperado = JugadorType.Jugador1;
                    break;
                case 2: ganadorEsperado = JugadorType.Jugador2;
                    break;
                default:
                    ganadorEsperado = JugadorType.None;
                    break;
            }
            var actual = resultados.UltimoGanador();
            Assert.AreEqual<JugadorType>(ganadorEsperado, actual);
                
        }
    }

    
}
