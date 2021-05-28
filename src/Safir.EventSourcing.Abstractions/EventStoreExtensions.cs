using System.Collections.Generic;
using System.Threading;
using JetBrains.Annotations;

namespace Safir.EventSourcing
{
    [PublicAPI]
    public static class EventStoreExtensions
    {
        public static IAsyncEnumerable<Event> StreamAsync(
            this IEventStore store,
            long aggregateId,
            CancellationToken cancellationToken)
        {
            return store.StreamAsync(aggregateId, cancellationToken: cancellationToken);
        }

        public static IAsyncEnumerable<Event> StreamAsync(
            this IEventStore store,
            long aggregateId,
            int startPosition,
            CancellationToken cancellationToken = default)
        {
            return store.StreamFromAsync(aggregateId, startPosition, cancellationToken);
        }

        public static IAsyncEnumerable<Event> StreamFromAsync(
            this IEventStore store,
            long aggregateId,
            int startPosition,
            CancellationToken cancellationToken = default)
        {
            return store.StreamAsync(aggregateId, startPosition, cancellationToken: cancellationToken);
        }

        public static IAsyncEnumerable<Event> StreamUntilAsync(
            this IEventStore store,
            long aggregateId,
            int endPosition,
            CancellationToken cancellationToken = default)
        {
            return store.StreamAsync(aggregateId, endPosition: endPosition, cancellationToken: cancellationToken);
        }
    }
}
