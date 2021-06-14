using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Safir.Messaging;

namespace Safir.EventSourcing
{
    [PublicAPI]
    public abstract class EventStore : IEventStore
    {
        public abstract Task AddAsync<TAggregateId>(
            TAggregateId aggregateId,
            IEvent @event,
            CancellationToken cancellationToken = default);

        public virtual Task AddAsync<TAggregateId>(
            TAggregateId aggregateId,
            IEnumerable<IEvent> events,
            CancellationToken cancellationToken = default)
        {
            return Task.WhenAll(events.Select(x => AddAsync(aggregateId, x, cancellationToken)));
        }

        public abstract Task<IEvent> GetAsync<TId>(TId id, CancellationToken cancellationToken = default);

        public virtual async Task<T> GetAsync<T, TId>(TId id, CancellationToken cancellationToken = default)
            where T : IEvent
        {
            return (T)await GetAsync(id, cancellationToken);
        }

        public virtual IAsyncEnumerable<IEvent> StreamBackwardsAsync<TAggregateId>(
            TAggregateId aggregateId,
            int? count = null,
            CancellationToken cancellationToken = default)
        {
            return StreamAsync(aggregateId, int.MaxValue, count ?? int.MinValue, cancellationToken);
        }

        public abstract IAsyncEnumerable<IEvent> StreamAsync<TAggregateId>(
            TAggregateId aggregateId,
            int startPosition = 0,
            int endPosition = int.MaxValue,
            CancellationToken cancellationToken = default);
    }
}
