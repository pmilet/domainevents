using pmilet.DomainEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiedraPapelTijeraApp
{
    public class Resultados : IHandleDomainEvents<PartidaFinalizada>
    {
        JugadorType _ultimoGanador;
        private readonly IDomainEventBus _bus;
        public Resultados(IDomainEventBus bus)
        {
            _bus = bus;
            bus.Subscribe<PartidaFinalizada>(this);
        }

        public Guid SubscriberId => throw new NotImplementedException();

        public void HandleEvent(PartidaFinalizada domainEvent)
        {
            _ultimoGanador = domainEvent.Ganador;
        }

        public JugadorType UltimoGanador()
        {
            return _ultimoGanador;
        }

    }
}
