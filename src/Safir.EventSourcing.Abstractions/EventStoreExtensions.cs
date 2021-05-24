using System.Collections.Generic;
using System.Threading;
using JetBrains.Annotations;
using Safir.Messaging;

namespace Safir.EventSourcing
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public static class EventStoreExtensions
    {
        public static IAsyncEnumerable<IEvent> GetStreamAsync(
            this IEventStore store,
            long aggregateId,
            CancellationToken cancellationToken)
        {
            return store.GetStreamAsync(aggregateId, cancellationToken: cancellationToken);
        }

        public static IAsyncEnumerable<IEvent> GetStreamAsync(
            this IEventStore store,
            long aggregateId,
            ulong startPosition,
            CancellationToken cancellationToken = default)
        {
            return store.GetStreamFromAsync(aggregateId, startPosition, cancellationToken);
        }

        public static IAsyncEnumerable<IEvent> GetStreamFromAsync(
            this IEventStore store,
            long aggregateId,
            ulong startPosition,
            CancellationToken cancellationToken = default)
        {
            return store.GetStreamAsync(aggregateId, startPosition, cancellationToken: cancellationToken);
        }

        public static IAsyncEnumerable<IEvent> GetStreamUntilAsync(
            this IEventStore store,
            long aggregateId,
            ulong endPosition,
            CancellationToken cancellationToken = default)
        {
            return store.GetStreamAsync(aggregateId, endPosition: endPosition, cancellationToken: cancellationToken);
        }
    }
}
