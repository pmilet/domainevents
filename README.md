### Domain Events

Martin Fowlers defines this DDD pattern [as:](https://martinfowler.com/eaaDev/DomainEvent.html)

Domain Events captures the memory of something interesting which affects the domain.

The essence of a Domain Event is that you use it to capture things that can trigger a change to the state of the application you are developing. These event objects are then processed to cause changes to the system.

This code is based on an article from Jimmy Boggard [A better domain events pattern.](https://lostechies.com/jimmybogard/2014/05/13/a-better-domain-events-pattern/) has been the inspiration for this library.

## How to use it

To create your DomainEvent class you could either inherit from the base Class DomainEvent or implement the IDomainEvent interface.

```csharp
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
```

To be able to publish events and subscribe to events our domain classes should use a DomainEventsBus instance that we can inject into their constructor: 

```cs
   IDomainEventBus bus = new DomainEventBus();
   Jugador j1 = new Jugador1(bus);
   Jugador j2 = new Jugador2(bus);
   Partida partida = new Partida(bus);
   Resultados resultados = new Resultados(bus);
   ```
  To trigger an event immediately we should use the DomainEventBus publish method informing the type :
  
  ```cs
  _bus.Publish<PartidaFinalizada>(new PartidaFinalizada(JugadorType.Jugador1));
 ```
 To record a delayed event we should use the add method, informing the type :
 
  ```cs
 _bus.Add<JugadaRealizada>(new JugadaRealizada( _jugador, jugada ));
 ```
To trigger all the delayed events we should use the commit method, informing the type (only the delayed events of this type will be triggered):
```cs
 _bus.Commit<JugadaRealizada>();
```
To subscribe a domain class to a specific event we should inherit our class from the IHandleEvent typed interface and subscribe to the bus
```cs
 public class Resultados : IHandleDomainEvents<PartidaFinalizada>
    {
```
      ...
```cs
        public Resultados(IDomainEventBus bus)
        {
            _bus = bus;
            bus.Subscribe<PartidaFinalizada>(this);
        }

```
      ...


