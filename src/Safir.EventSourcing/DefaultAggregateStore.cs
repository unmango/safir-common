using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Safir.Common;
using Safir.Messaging;

namespace Safir.EventSourcing
{
    public class DefaultAggregateStore<TAggregate> : IAggregateStore<TAggregate>, IAggregateStore
        where TAggregate : IAggregate
    {
        private readonly IEventStore _store;
        private readonly ISerializer _serializer;
        private readonly ILogger<DefaultAggregateStore<TAggregate>> _logger;

        public DefaultAggregateStore(
            IEventStore store,
            ISerializer serializer,
            ILogger<DefaultAggregateStore<TAggregate>> logger)
        {
            _store = store ?? throw new ArgumentNullException(nameof(store));
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task StoreAsync(TAggregate aggregate, CancellationToken cancellationToken = default)
        {
            return StoreAsync<TAggregate>(aggregate, cancellationToken);
        }

        public async Task StoreAsync<T>(T aggregate, CancellationToken cancellationToken = default)
            where T : IAggregate
        {
            var tasks = aggregate.DequeueAllEvents().Select(e => SerializeAsync(e, cancellationToken));
            
            var events = await Task.WhenAll(tasks);
            await _store.AddAsync(events, cancellationToken);
        }

        public ValueTask<TAggregate> GetAsync(long id, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public ValueTask<T> GetAsync<T>(long id, CancellationToken cancellationToken = default)
            where T : IAggregate
        {
            throw new System.NotImplementedException();
        }

        public ValueTask<TAggregate> GetAsync(long id, int version, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public ValueTask<T> GetAsync<T>(long id, int version, CancellationToken cancellationToken = default)
            where T : IAggregate
        {
            throw new System.NotImplementedException();
        }

        private async Task<Event> SerializeAsync<T>(T @event, CancellationToken cancellationToken)
            where T : IEvent
        {
            var data = await _serializer.SerializeAsMemoryAsync(@event, cancellationToken);
        }
    }
}
