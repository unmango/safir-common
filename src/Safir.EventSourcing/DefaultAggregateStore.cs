using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Safir.EventSourcing
{
    public class DefaultAggregateStore : IAggregateStore
    {
        private readonly IEventStore _store;
        private readonly ILogger<DefaultAggregateStore> _logger;

        public DefaultAggregateStore(IEventStore store, ILogger<DefaultAggregateStore> logger)
        {
            _store = store ?? throw new ArgumentNullException(nameof(store));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task StoreAsync<T>(T aggregate, CancellationToken cancellationToken = default)
            where T : IAggregate
        {
            _logger.LogTrace("Adding events to event store");
            return _store.AddAsync(aggregate.DequeueEvents(), cancellationToken);
        }

        public ValueTask<T> GetAsync<T>(long id, CancellationToken cancellationToken = default)
            where T : IAggregate, new()
        {
            // TODO: This cancellation token situation...
            return _store.StreamAsync(id, cancellationToken)
                .AggregateAsync<T>(cancellationToken);
        }

        public ValueTask<T> GetAsync<T>(long id, int version, CancellationToken cancellationToken = default)
            where T : IAggregate, new()
        {
            // TODO: This cancellation token situation...
            return _store.StreamAsync(id, version, cancellationToken)
                .AggregateAsync<T>(cancellationToken);
        }
    }
}
