using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Safir.EventSourcing
{
    [PublicAPI]
    public abstract class EventStore : IEventStore
    {
        public abstract Task AddAsync(Event @event, CancellationToken cancellationToken = default);

        public virtual Task AddAsync(IEnumerable<Event> events, CancellationToken cancellationToken = default)
        {
            return Task.WhenAll(events.Select(x => AddAsync(x, cancellationToken)));
        }

        public abstract Task<Event> GetAsync(long id, CancellationToken cancellationToken = default);

        public virtual IAsyncEnumerable<Event> StreamBackwardsAsync(
            long aggregateId,
            int count,
            CancellationToken cancellationToken = default)
        {
            return this.StreamAsync(aggregateId, count, cancellationToken).Reverse().Take(count);
        }

        public abstract IAsyncEnumerable<Event> StreamAsync(
            long aggregateId,
            int startPosition = int.MinValue,
            int endPosition = int.MaxValue,
            CancellationToken cancellationToken = default);
    }
}
