using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Safir.Common;
using Safir.Messaging;

namespace Safir.EventSourcing
{
    public abstract class EventStore : IEventStore
    {
        private readonly ISerializer _serializer;
        private readonly IEventMetadataProvider _metadataProvider;

        protected EventStore()
        {
        }
        
        protected EventStore(ISerializer serializer, IEventMetadataProvider metadataProvider)
        {
            _serializer = serializer;
            _metadataProvider = metadataProvider;
        }
        
        public Task AddAsync<T>(
            long aggregateId,
            T @event,
            DateTime occurred,
            Guid correlationId,
            Guid causationId,
            int version,
            CancellationToken cancellationToken = default) where T : IEvent
        {
            throw new NotImplementedException();
        }

        public Task<IEvent> GetAsync(long id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetAsync<T>(long id, CancellationToken cancellationToken = default) where T : IEvent
        {
            throw new NotImplementedException();
        }

        public IAsyncEnumerable<IEvent> GetStreamAsync(
            long aggregateId,
            ulong startPosition = ulong.MinValue,
            ulong endPosition = ulong.MaxValue,
            CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        protected abstract ValueTask AddAsync(Event @event, CancellationToken cancellationToken);

        protected abstract IAsyncEnumerable<Event> GetStreamAsync(CancellationToken cancellationToken);
    }
}
