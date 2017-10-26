### Domain Events
The purppose of this library is to simplify the development of applications using Domain Events.
This library is based on an article from Jimmy Boggard [A better domain events pattern.](https://lostechies.com/jimmybogard/2014/05/13/a-better-domain-events-pattern/).

Martin Fowlers defines the domainEvent DDD pattern as: Domain Events [captures the memory of something interesting which affects the domain](https://martinfowler.com/eaaDev/DomainEvent.html).
The essence of a Domain Event is that you use it to capture important things that happens into the domain and that can produce a change into the state of the application you are developing.

Another good source of information about Domain Events is this [chapter](https://docs.microsoft.com/en-us/dotnet/standard/microservices-architecture/microservice-ddd-cqrs-patterns/domain-events-design-implementation) from the Microsoft [ebook](https://docs.microsoft.com/en-us/dotnet/standard/microservices-architecture/): .NET Microservices. Architecture for Containerized .NET Applications

## 1. Model your domain event

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
## 2. Create an instance of the DomainEventDispatcher

From your business logic you will publish events or subscribe to events by using a DomainEventDispatcher instance. 

```cs
  // we create the domain event dispatcher and inject it into the objects of our domain model (normally done using a IoC container) 
  IDomainEventDispatcher dispatcher = new DomainEventDispatcher();
  Player j1 = new Player1(dispatcher);
  Player j2 = new Player2(dispatcher);
  Match match = new Match(dispatcher);
  Outcome outcome = new Outcome(dispatcher);
   ```
  Note: Normally you will register the DomainEventDispatcher instance in your IoC container. 
  
  ## 3. Trigger domain events from your business logic 
  
  To trigger an event immediately we should use the Publish method:
  
  ```cs
    //publish an event notifying that the match ended and Player1 is the winner
    _bus.Publish<MatchEnded>(new MatchEnded(PlayerType.Player1));
 ```
 To add a delayed event we should use the Add method :
 
  ```cs
    //delayed event to notify of the move choosen by the player
    _bus.Add<PlayMade>(new PlayMade( _player, play ));
```
To trigger all the delayed events we should use the Commit method (only the delayed events of the specified type will be triggered):
```cs
    //commit all registered delayed events
    _bus.Commit<PlayMade>();
```

## 4. Respond to specific domain events from your business logic by subscribing to them  

To subscribe to specific type of domain events we should inherit from the IHandleDomainEvents<T> interface and subscribe to the dispatcher instance (normally in the constructor).

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

## See a sample app that uses domain events 
    
    To see a running sample take a look to the StonePaperScissors app or specflow functional tests from the [github](https://github.com/pmilet/domainevents) repo 
    



