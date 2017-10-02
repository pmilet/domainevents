### Domain Events

Martin Fowlers defines this DDD pattern as:

Domain Events [captures the memory of something interesting which affects the domain](https://martinfowler.com/eaaDev/DomainEvent.html).

The essence of a Domain Event is that you use it to capture things that can trigger a change to the state of the application you are developing. These event objects are then processed to cause changes to the system.

This package is based on an article from Jimmy Boggard [A better domain events pattern.](https://lostechies.com/jimmybogard/2014/05/13/a-better-domain-events-pattern/) has been the inspiration for this library.

## How to use it

To create your DomainEvent class you could either inherit from the base Class DomainEvent or implement the IDomainEvent interface.

```csharp
    /// <summary>
    /// Represents the play choosen by a player
    /// </summary>
    public class PlayMade : DomainEvent
    {
        public PlayMade(PlayerType player, PlayType play)
            : base( "PlayMade", "1.0")
        {
            Player = player;
            Play = play;
        }

        public PlayerType Player { get; private set; }
        public PlayType Play { get; private set; }

    }
```

To be able to publish events and subscribe to events our domain objects will use a DomainEventBus instance inject into the constructor: 

```cs
  // we create the domain event bus and inject it into the objects of our domain model (normally done using a IoC container) 
  IDomainEventBus bus = new DomainEventBus();
  Player j1 = new Player1(bus);
  Player j2 = new Player2(bus);
  Match match = new Match(bus);
  Outcome outcome = new Outcome(bus);
   ```
  To trigger an event immediately we should use the typed Publish method:
  
  ```cs
    //publish an event notifying that the match ended and Player1 is the winner
    _bus.Publish<MatchEnded>(new MatchEnded(PlayerType.Player1));
 ```
 To record a delayed event we should use the typed Add method :
 
  ```cs
    //delayed event to notify of the move choosen by the player
    _bus.Add<PlayMade>(new PlayMade( _player, play ));
```
To trigger all the delayed events we should use the typed Commit method (only the delayed events of the type will be triggered):
```cs
    //commit all registered delayed events
    _bus.Commit<PlayMade>();
```
To subscribe a domain object to handle specifics events we should inherit from the IHandleEvent interface and subscribe to the bus
(note that we could subscribe to one or more type of events by just inheriting to the IHandleEvent of the specific Type) 
```cs
    public class Outcome : IHandleDomainEvents<MatchEnded>
    {
        PlayerType _lastWinner;
        private readonly IDomainEventBus _bus;
        public Outcome(IDomainEventBus bus)
        {
            _bus = bus;
            bus.Subscribe<MatchEnded>(this);
        }

        public Guid SubscriberId => throw new NotImplementedException();

        public void HandleEvent(MatchEnded domainEvent)
        {
            _lastWinner = domainEvent.Winner;
        }

        public PlayerType LastWinner()
        {
            return _lastWinner;
        }

    }
    ```
    To see a running sample take a look to the StonePaperScissors specflow test example:
    



