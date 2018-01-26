using pmilet.DomainEvents;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pmilet.DomainEvents
{
    [EventSource(Name = "pmilet.DomainEvents")]
    public sealed class DomainEventSource : EventSource
    {
        public static readonly DomainEventSource Current = new DomainEventSource();

        static DomainEventSource()
        {
            // A workaround for the problem where ETW activities do not get tracked until Tasks infrastructure is initialized.
            // This problem will be fixed in .NET Framework 4.6.2.
            Task.Run(() => { });
        }

        private DomainEventSource() : base() { }

        [NonEvent]
        public void Log(IDomainEvent domainEvent)
        {
            if (this.IsEnabled())
            {
                string payload = JsonConvert.SerializeObject(domainEvent);
                LogEvent( domainEvent.AggregateSource, domainEvent.Version, payload );
            }
        }


        [Event(1, Level = EventLevel.Informational, Message = "{0}")]
        public void LogEvent(string source, string version, string payload )
        {
            if (this.IsEnabled())
            {
                WriteEvent(1, source, version, payload );
            }
        }

        [NonEvent]
        public void Log(string message)
        {
            if (this.IsEnabled())
            {
                LogInformation(message);
            }
        }

        [Event(2, Level = EventLevel.Informational, Message = "{0}")]
        public void LogInformation(string message)
        {
            if (this.IsEnabled())
            {
                WriteEvent(2, message);
            }
        }
    }
}
