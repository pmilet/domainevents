### Purpose
The purppose of this library is to simplify the development of applications that use the Domain Event design pattern.
This library is based on an article from Jimmy Boggard [A better domain events pattern.](https://lostechies.com/jimmybogard/2014/05/13/a-better-domain-events-pattern).

### Domain Events
Martin Fowlers defines the domainEvent DDD pattern as: Domain Events [captures the memory of something interesting which affects the domain](https://martinfowler.com/eaaDev/DomainEvent.html).
The essence of a Domain Event is that you use it to capture important things that happens into the domain and that can produce a change into the state of the application you are developing.
Another good source of information is this [chapter](https://docs.microsoft.com/en-us/dotnet/standard/microservices-architecture/microservice-ddd-cqrs-patterns/domain-events-design-implementation) about Domain Events from the Microsoft [ebook](https://docs.microsoft.com/en-us/dotnet/standard/microservices-architecture/): .NET Microservices. Architecture for Containerized .NET Applications

### 1. Model your domain event

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
### 2. Create an instance of the DomainEventDispatcher

From your business logic you can publish events or subscribe to events by means of a DomainEventDispatcher instance. 

```cs
  // we create the domain event dispatcher and inject it into the objects of our domain model (normally done using a IoC container) 
  IDomainEventDispatcher dispatcher = new DomainEventDispatcher();
  Player j1 = new Player1(dispatcher);
  Player j2 = new Player2(dispatcher);
  Match match = new Match(dispatcher);
  Outcome outcome = new Outcome(dispatcher);
   ```
  Note: a better option is to register the DomainEventDispatcher instance in your IoC container. 
  
### 3. Trigger domain events from your business logic 
  
  To trigger an domain event immediately we should use the DomainEventDipatcher Publish<T> method:
  
  ```cs
    //publish an event notifying that the match ended and Player1 is the winner
    _dispatcher.Publish<MatchEnded>(new MatchEnded(PlayerType.Player1));
 ```
 To add a delayed domain event we should use the Add<T> method :
 
  ```cs
    //delayed event to notify of the move choosen by the player
    _dispatcher.Add<PlayMade>(new PlayMade( _player, play ));
```
To trigger all the delayed domain events of a specific type we should use the Commit<T> method :
```cs
    //commit all registered delayed events
    _dispatcher.Commit<PlayMade>();
```

### 4. Respond to specific domain events from your business logic by subscribing to them  

The easiest way to subscribe to a specific domain event is by inheriting from the HandleDomainEventsBase<T> class.
This way the subscription is done automatically, you just have to override the HandleEvent method as shown:

```cs
    public class Outcome : HandleDomainEventsBase<MatchEnded>
    {
        PlayerType _lastWinner;
        public Outcome(IDomainEventDispatcher dispatcher):base(dispatcher)
        {}

        public override void HandleEvent(MatchEnded domainEvent)
        {
            _lastWinner = domainEvent.Winner;
        }
        
        public PlayerType LastWinner()
        {
            return _lastWinner;
        }

    }
    ```
If you want a class to subscribe to several domain events (this may be breaks the SR principle ) we should inherit from  IHandleDomainEvents<T> interface and subscribe explicitly. In this example we combine both approachs.

```cs
    public class Match : HandleDomainEventsBase<PlayMade>, 
        IHandleDomainEvents<InvalidPlay>
    {
     ...

        public Match( IDomainEventDispatcher dispatcher):base( dispatcher)
        {
            domainEventDispatcher = dispatcher;
            dispatcher.Subscribe<InvalidPlay>(this);
        }
        
        ...
        
        public override void HandleEvent(PlayMade domainEvent)
        {
            SavePlay(domainEvent);

            EvalOutcome();
        }

        public void HandleEvent(InvalidPlay domainEvent)
        {
            Console.WriteLine($"invalid play {domainEvent.Play.ToString()} made by {domainEvent.Player.ToString()}");
        }
    }
    ```

### See a example app that uses domain events 
    
    To see a full sample take a look to the StonePaperScissors app or specflow functional tests from the [github](https://github.com/pmilet/domainevents) repo 
    



