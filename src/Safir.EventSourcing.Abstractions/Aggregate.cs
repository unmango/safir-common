using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Safir.Messaging;

namespace Safir.EventSourcing
{
    [PublicAPI]
    public abstract record Aggregate : IAggregate
    {
        public long Id { get; protected set; }
        
        public ulong Version { get; protected set; }
        
        public IEnumerable<IEvent> Events { get; protected set; } = Enumerable.Empty<IEvent>();

        public virtual void Apply<T>(T @event) where T : IEvent { }
    }
}
