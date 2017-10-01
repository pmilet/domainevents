using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using pmilet.DomainEvents;

namespace PiedraPapelTijeraApp.Tests
{
    [TestClass]
    public class PiedraPapelTijeraTest
    {

        IDomainEventBus _bus;
        [TestInitialize]
        public void Setup()
        {
            _bus = new DomainEventBus();
        }

        [TestMethod]
        public void PiedraGanaATijera()
        {
            Partida partida = new Partida(_bus);
            Resultados resultados = new Resultados(_bus);
            _bus.Publish<JugadaRealizada>(new JugadaRealizada(JugadorType.Jugador1, Jugada.Piedra));
            _bus.Publish<JugadaRealizada>(new JugadaRealizada(JugadorType.Jugador2, Jugada.Tijera));
            partida.FinalizarPartida();
            Assert.AreEqual<JugadorType>(JugadorType.Jugador1, resultados.UltimoGanador());
        }
    }
}
