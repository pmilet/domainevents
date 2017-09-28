using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PiedraPapelTijeraApp.Tests
{
    [TestClass]
    public class PiedraPapelTijeraTest
    {

        pmilet.DomainEvents.IDomainEventBus _bus;
        [TestInitialize]
        public void Setup()
        {
            _bus = new pmilet.DomainEvents.DomainEventBus();
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
