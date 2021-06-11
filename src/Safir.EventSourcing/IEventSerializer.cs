using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Safir.Messaging;

namespace Safir.EventSourcing
{
    [PublicAPI]
    public interface IEventSerializer
    {
        ValueTask<Event<TAggregateId, TId>> SerializeAsync<TAggregateId, TId>(
            TAggregateId aggregateId,
            IEvent @event,
            CancellationToken cancellationToken = default);

        ValueTask<IEvent> DeserializeAsync<TAggregateId, TId>(
            Event<TAggregateId, TId> @event,
            CancellationToken cancellationToken = default);

        ValueTask<T> DeserializeAsync<TAggregateId, TId, T>(
            Event<TAggregateId, TId> @event,
            CancellationToken cancellationToken = default)
            where T : IEvent;
    }
}
