using System;

namespace pierreml.DomainEvents
{
    public class DomainEvent : IDomainEvent
    {
        public DomainEvent(string source, string version)
        {
            AggregateSource = source;
            Version = version;
            CreatedOn = DateTime.Now;
        }

        public string AggregateSource
        {
            get; private set;
        }

        public DateTime CreatedOn
        {
            get; private set;
        }

        public string Version
        {
            get; private set;
        }
    }
}
