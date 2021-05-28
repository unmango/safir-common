using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Safir.EventSourcing.EntityFrameworkCore
{
    public class DbContextEventStore<T> : IEventStore
        where T : DbContext
    {
        private readonly T _context;
        private readonly ILogger<DbContextEventStore<T>> _logger;

        public DbContextEventStore(T context, ILogger<DbContextEventStore<T>> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task AddAsync(Event @event, CancellationToken cancellationToken = default)
        {
            _logger.LogTrace("Adding event {Event}", @event);
            await _context.AddAsync(@event, cancellationToken);
            _logger.LogTrace("Added event {Event}", @event);
            
            _logger.LogTrace("Saving changes asynchronously");
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogTrace("Saved changes asynchronously");
        }

        public async Task AddAsync(IEnumerable<Event> events, CancellationToken cancellationToken = default)
        {
            _logger.LogTrace("Adding events {Events}", events);
            await _context.AddRangeAsync(events, cancellationToken);
            _logger.LogTrace("Added events {Events}", events);
            
            _logger.LogTrace("Saving changes asynchronously");
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogTrace("Saved changes asynchronously");
        }

        public Task<Event> GetAsync(long id, CancellationToken cancellationToken = default)
        {
            _logger.LogTrace("Getting single event with id {Id}", id);
            return _context.Set<Event>().AsQueryable().FirstAsync(x => x.Id == id, cancellationToken);
        }

        public IAsyncEnumerable<Event> StreamBackwardsAsync(
            long aggregateId,
            int count,
            CancellationToken cancellationToken = default)
        {
            _logger.LogTrace("Streaming {Count} events backwards with aggregate Id {Id}", count, aggregateId);
            return _context.Set<Event>().AsAsyncEnumerable().Reverse().Take(count);
        }

        public IAsyncEnumerable<Event> StreamAsync(
            long aggregateId,
            int startPosition = int.MinValue,
            int endPosition = int.MaxValue,
            CancellationToken cancellationToken = default)
        {
            _logger.LogTrace(
                "Streaming events with aggregateId {AggregateId} from start {Start} till end {End}",
                aggregateId,
                startPosition,
                endPosition);
            
            return _context.Set<Event>().AsAsyncEnumerable().Where(Matches);

            bool Matches(Event @event) =>
                @event.AggregateId == aggregateId &&
                @event.Position >= startPosition &&
                @event.Position <= endPosition;
        }
    }
}
