using System;
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
        Task AddAsync<TAggregateId>(TAggregateId aggregateId, IEvent @event, CancellationToken cancellationToken = default);

        Task AddAsync<TAggregateId>(TAggregateId aggregateId, IEnumerable<IEvent> events, CancellationToken cancellationToken = default);

        Task<IEvent> GetAsync<TId>(TId id, CancellationToken cancellationToken = default);

        Task<T> GetAsync<T, TId>(TId id, CancellationToken cancellationToken = default)
            where T : IEvent;

        IAsyncEnumerable<IEvent> StreamAsync<TAggregateId>(
            TAggregateId aggregateId,
            int startPosition = 0,
            int endPosition = int.MaxValue,
            CancellationToken cancellationToken = default);

        IAsyncEnumerable<IEvent> StreamBackwardsAsync<TAggregateId>(
            TAggregateId aggregateId,
            int? count = null,
            CancellationToken cancellationToken = default);
    }
}
