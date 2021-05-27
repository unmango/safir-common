using System.Collections.Generic;
using JetBrains.Annotations;
using Safir.Messaging;

namespace Safir.EventSourcing
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public interface IAggregate
    {
        long Id { get; }
        
        ulong Version { get; }
        
        IEnumerable<IEvent> Events { get; }

        void Apply<T>(T @event)
            where T : IEvent;
    }
}
