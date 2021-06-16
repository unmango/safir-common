using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Safir.Messaging;

namespace Safir.EventSourcing
{
    [PublicAPI]
    public interface IEventStore
    {
        // TODO: Accept generic event?
        Task AddAsync<TAggregateId, TId>(TAggregateId aggregateId, IEvent @event, CancellationToken cancellationToken = default);

        Task AddAsync<TAggregateId, TId>(
            TAggregateId aggregateId,
            IEnumerable<IEvent> events,
            CancellationToken cancellationToken = default);

        Task<IEvent> GetAsync<TAggregateId, TId>(TId id, CancellationToken cancellationToken = default);

        Task<TEvent> GetAsync<TEvent, TAggregateId, TId>(TId id, CancellationToken cancellationToken = default)
            where TEvent : IEvent;

        IAsyncEnumerable<IEvent> StreamAsync<TAggregateId, TId>(
            TAggregateId aggregateId,
            int startPosition = 0,
            int endPosition = int.MaxValue,
            CancellationToken cancellationToken = default);

        IAsyncEnumerable<IEvent> StreamBackwardsAsync<TAggregateId, TId>(
            TAggregateId aggregateId,
            int? count = null,
            CancellationToken cancellationToken = default);
    }
}
