using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pmilet.DomainEvents
{
    public interface IHandleDomainEventsBase
    {
        Guid SubscriberId { get; }
        void HandleDomainEvent(IDomainEvent domainEvent);
    }
}
