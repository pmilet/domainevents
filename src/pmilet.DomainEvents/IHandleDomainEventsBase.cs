namespace pmilet.DomainEvents
{
    public interface IHandleDomainEventsBase
    {
        void HandleDomainEvent(IDomainEvent domainEvent);
    }
}
