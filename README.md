### Domain Events

Martin Fowlers defines the domainEvent DDD pattern as: Domain Events [captures the memory of something interesting which affects the domain](https://martinfowler.com/eaaDev/DomainEvent.html).

The essence of a Domain Event is that you use it to capture important things that happens into the domain that can produce a change into the state of the application you are developing.

This package is based on an article from Jimmy Boggard [A better domain events pattern.](https://lostechies.com/jimmybogard/2014/05/13/a-better-domain-events-pattern/).

## How to use it


Install-Package pmilet.DomainEvents -Version 1.0.3


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

To be able to publish events and subscribe to events our domain objects will use a DomainEventDispatcher instance injected into the constructor: 

```cs
  // we create the domain event dispatcher and inject it into the objects of our domain model (normally done using a IoC container) 
  IDomainEventDispatcher dispatcher = new DomainEventDispatcher();
  Player j1 = new Player1(dispatcher);
  Player j2 = new Player2(dispatcher);
  Match match = new Match(dispatcher);
  Outcome outcome = new Outcome(dispatcher);
   ```
  To trigger an event immediately we should use the Publish method:
  
  ```cs
    //publish an event notifying that the match ended and Player1 is the winner
    _bus.Publish<MatchEnded>(new MatchEnded(PlayerType.Player1));
 ```
 To record a delayed event we should use the Add method :
 
  ```cs
    //delayed event to notify of the move choosen by the player
    _bus.Add<PlayMade>(new PlayMade( _player, play ));
```
To trigger all the delayed events we should use the Commit method (only the delayed events of the specified type will be triggered):
```cs
    //commit all registered delayed events
    _bus.Commit<PlayMade>();
```
To subscribe a domain object to handle specifics events we should inherit from the IHandleEvent interface and subscribe to the dispatcher
(note that we could subscribe to one or more type of events by just inheriting to the IHandleEvent of the specific Type) 
```cs
    public class Outcome : IHandleDomainEvents<MatchEnded>
    {
        PlayerType _lastWinner;
        private readonly IDomainEventDispatcher _bus;
        public Outcome(IDomainEventDispatcher dispatcher)
        {
            _bus = dispatcher;
            dispatcher.Subscribe<MatchEnded>(this);
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
    To see a running sample take a look to the StonePaperScissors specflow test example 
    



