using System;
using System.Buffers;
using System.Collections.Generic;
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
        private readonly IEventTypeProvider _eventTypeProvider;

        public DbContextEventStore(TContext context, ISerializer serializer, IEventTypeProvider eventTypeProvider)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
            _eventTypeProvider = eventTypeProvider ?? throw new ArgumentNullException(nameof(eventTypeProvider));
        }

        public async Task AddAsync<T>(T @event, CancellationToken cancellationToken = default) where T : IEvent
        {
            var writer = new ArrayBufferWriter<byte>();
            await _serializer.SerializeAsync(writer, @event, cancellationToken);

            var type = _eventTypeProvider.GetType(@event);
            
            // TODO: How to get some of these values
            var entity = new Event(
                type,
                0,
                writer.WrittenMemory,
                new Metadata(Guid.Empty, Guid.Empty),
                69);

            await _context.AddAsync(entity, cancellationToken);
        }

        public async Task<IEvent> GetAsync(long id, CancellationToken cancellationToken = default)
        {
            var @event = await _context.Set<Event>().FirstAsync(x => x.Id == id, cancellationToken);

            // Use Type property to get type to deserialize as?
            throw new NotImplementedException();
        }

        public async Task<T> GetAsync<T>(long id, CancellationToken cancellationToken = default) where T : IEvent
        {
            var @event = await _context.Set<Event>().FirstAsync(x => x.Id == id, cancellationToken);
            return await _serializer.DeserializeAsync<T>(@event.Data, cancellationToken);
        }

        public IAsyncEnumerable<IEvent> GetStreamAsync(
            long aggregateId,
            ulong startPosition = ulong.MinValue,
            ulong endPosition = ulong.MaxValue,
            CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
