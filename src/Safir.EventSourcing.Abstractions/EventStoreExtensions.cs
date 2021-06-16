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
    public static class EventStoreExtensions
    {
        public static Task<T> CreateAsync<T>(
            this IEventStore store,
            IEvent @event,
            CancellationToken cancellationToken = default)
            where T : IAggregate, new()
        {
            return store.CreateAsync<T, Guid>(@event, cancellationToken);
        }
        
        public static Task<TAggregate> CreateAsync<TAggregate, TId>(
            this IEventStore store,
            IEvent @event,
            CancellationToken cancellationToken = default)
            where TAggregate : IAggregate<TId>, new()
        {
            return store.CreateAsync<TAggregate, TId, Guid>(@event, cancellationToken);
        }
        
        public static async Task<TAggregate> CreateAsync<TAggregate, TAggregateId, TId>(
            this IEventStore store,
            IEvent @event,
            CancellationToken cancellationToken = default)
            where TAggregate : IAggregate<TAggregateId>, new()
        {
            var aggregate = new TAggregate();
            await store.AddAsync<TAggregateId, TId>(aggregate.Id, @event, cancellationToken);
            aggregate.Apply(@event);
            return aggregate;
        }
        
        public static Task<T> CreateAsync<T>(
            this IEventStore store,
            IEnumerable<IEvent> events,
            CancellationToken cancellationToken = default)
            where T : IAggregate, new()
        {
            return store.CreateAsync<T, Guid>(events, cancellationToken);
        }
        
        public static Task<TAggregate> CreateAsync<TAggregate, TId>(
            this IEventStore store,
            IEnumerable<IEvent> events,
            CancellationToken cancellationToken = default)
            where TAggregate : IAggregate<TId>, new()
        {
            return store.CreateAsync<TAggregate, TId, Guid>(events, cancellationToken);
        }
        
        public static async Task<TAggregate> CreateAsync<TAggregate, TAggregateId, TId>(
            this IEventStore store,
            IEnumerable<IEvent> events,
            CancellationToken cancellationToken = default)
            where TAggregate : IAggregate<TAggregateId>, new()
        {
            var aggregate = new TAggregate();
            var list = events.ToList();
            await store.AddAsync<TAggregateId, TId>(aggregate.Id, list, cancellationToken);
            list.ForEach(aggregate.Apply);
            return aggregate;
        }

        public static async Task<Guid> NewAsync(
            this IEventStore store,
            IEvent @event,
            CancellationToken cancellationToken = default)
        {
            var id = Guid.NewGuid();
            await store.AddAsync<Guid, Guid>(id, @event, cancellationToken);
            return id;
        }

        public static async Task<Guid> NewAsync<TId>(
            this IEventStore store,
            IEvent @event,
            CancellationToken cancellationToken = default)
        {
            var id = Guid.NewGuid();
            await store.AddAsync<Guid, TId>(id, @event, cancellationToken);
            return id;
        }

        public static async Task<Guid> NewAsync(
            this IEventStore store,
            IEnumerable<IEvent> events,
            CancellationToken cancellationToken = default)
        {
            var id = Guid.NewGuid();
            await store.AddAsync<Guid, Guid>(id, events, cancellationToken);
            return id;
        }

        public static async Task<Guid> NewAsync<TId>(
            this IEventStore store,
            IEnumerable<IEvent> events,
            CancellationToken cancellationToken = default)
        {
            var id = Guid.NewGuid();
            await store.AddAsync<Guid, TId>(id, events, cancellationToken);
            return id;
        }

        public static IAsyncEnumerable<IEvent> StreamAsync<TAggregateId, TId>(
            this IEventStore store,
            TAggregateId aggregateId,
            CancellationToken cancellationToken)
        {
            return store.StreamAsync<TAggregateId, TId>(aggregateId, cancellationToken: cancellationToken);
        }

        public static IAsyncEnumerable<IEvent> StreamAsync<TAggregateId, TId>(
            this IEventStore store,
            TAggregateId aggregateId,
            int startPosition,
            CancellationToken cancellationToken)
        {
            return store.StreamAsync<TAggregateId, TId>(aggregateId, startPosition, cancellationToken: cancellationToken);
        }

        public static IAsyncEnumerable<IEvent> StreamBackwardsAsync<TAggregateId, TId>(
            this IEventStore store,
            TAggregateId aggregateId,
            CancellationToken cancellationToken)
        {
            return store.StreamBackwardsAsync<TAggregateId, TId>(aggregateId, null, cancellationToken);
        }

        public static IAsyncEnumerable<IEvent> StreamFromAsync<TAggregateId, TId>(
            this IEventStore store,
            TAggregateId aggregateId,
            int startPosition,
            CancellationToken cancellationToken = default)
        {
            return store.StreamAsync<TAggregateId, TId>(aggregateId, startPosition, cancellationToken);
        }

        public static IAsyncEnumerable<IEvent> StreamUntilAsync<TAggregateId, TId>(
            this IEventStore store,
            TAggregateId aggregateId,
            int endPosition,
            CancellationToken cancellationToken = default)
        {
            return store.StreamAsync<TAggregateId, TId>(
                aggregateId,
                endPosition: endPosition,
                cancellationToken: cancellationToken);
        }
    }
}
