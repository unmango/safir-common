using System.Collections.Generic;
using JetBrains.Annotations;
using Safir.Messaging;

namespace Safir.EventSourcing
{
    [PublicAPI]
    public interface IAggregate
    {
        long Id { get; }
        
        int Version { get; }
        
        IEnumerable<IEvent> Events { get; }

        void Apply(object @event);

        IEnumerable<IEvent> DequeueAllEvents();
    }
}
