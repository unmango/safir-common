using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Safir.Common;
using Safir.Messaging;

namespace Safir.EventSourcing.EntityFrameworkCore
{
    public class DbContextEventStore<TContext> : IEventStore
        where TContext : DbContext
    {
        private readonly TContext _context;
        private readonly ISerializer _serializer;
        private readonly IEventMetadataProvider _eventMetadataProvider;

        public DbContextEventStore(TContext context, ISerializer serializer, IEventMetadataProvider eventMetadataProvider)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
            _eventMetadataProvider = eventMetadataProvider ?? throw new ArgumentNullException(nameof(eventMetadataProvider));
        }

        public async Task AddAsync<T>(
            long aggregateId,
            T @event,
            DateTime occurred,
            Guid correlationId,
            Guid causationId,
            int version,
            CancellationToken cancellationToken = default)
            where T : IEvent
        {
            var writer = new ArrayBufferWriter<byte>();
            await _serializer.SerializeAsync(writer, @event, cancellationToken);

            var type = await _eventMetadataProvider.GetTypeDiscriminatorAsync(@event, version, cancellationToken);

            var entity = new Event(
                type,
                aggregateId,
                writer.WrittenMemory,
                occurred,
                new(correlationId, causationId),
                version);

            await _context.AddAsync(entity, cancellationToken);
        }

        public async Task<IEvent> GetAsync(long id, CancellationToken cancellationToken = default)
        {
            var @event = await _context.Set<Event>().AsQueryable().FirstAsync(x => x.Id == id, cancellationToken);
            var type = await _eventMetadataProvider.GetTypeAsync(@event.Type, @event.Version, cancellationToken);
            return await DeserializeAsync(type, @event.Data, cancellationToken);
        }

        public async Task<T> GetAsync<T>(long id, CancellationToken cancellationToken = default) where T : IEvent
        {
            var @event = await _context.Set<Event>().AsQueryable().FirstAsync(x => x.Id == id, cancellationToken);
            return await _serializer.DeserializeAsync<T>(@event.Data, cancellationToken);
        }

        public IAsyncEnumerable<IEvent> GetStreamAsync(
            long aggregateId,
            ulong startPosition = ulong.MinValue,
            ulong endPosition = ulong.MaxValue,
            CancellationToken cancellationToken = default)
        {
            return _context.Set<Event>()
                .AsAsyncEnumerable()
                .Where(Matches)
                .SelectAwaitWithCancellation(Deserialize);

            bool Matches(Event @event) =>
                @event.AggregateId == aggregateId &&
                @event.Position >= startPosition &&
                @event.Position <= endPosition;

            async ValueTask<IEvent> Deserialize(Event @event, CancellationToken token)
            {
                var type = await _eventMetadataProvider.GetTypeAsync(@event.Type, @event.Version, token);
                return await DeserializeAsync(type, @event.Data, token);
            }
        }

        private async ValueTask<IEvent> DeserializeAsync(
            Type type,
            ReadOnlyMemory<byte> data,
            CancellationToken cancellationToken)
        {
            if (!typeof(IEvent).IsAssignableFrom(type))
                throw new InvalidOperationException($"Resolved type is not assignable to {nameof(IEvent)}");

            return (IEvent)await _serializer.DeserializeAsync(type, data, cancellationToken);
        }
    }
}
