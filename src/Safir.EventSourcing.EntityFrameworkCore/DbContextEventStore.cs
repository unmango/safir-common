using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Safir.EventSourcing.EntityFrameworkCore
{
    public class DbContextEventStore<TContext> : IEventStore
        where TContext : DbContext
    {
        private readonly TContext _context;

        public DbContextEventStore(TContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task AddAsync(Event @event, CancellationToken cancellationToken = default)
        {
            await _context.AddAsync(@event, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public Task AddAsync(IEnumerable<Event> events, CancellationToken cancellationToken = default)
        {
            return _context.AddRangeAsync(events, cancellationToken);
        }

        public Task<Event> GetAsync(long id, CancellationToken cancellationToken = default)
        {
            return _context.Set<Event>().AsQueryable().FirstAsync(x => x.Id == id, cancellationToken);
        }

        public IAsyncEnumerable<Event> StreamBackwardsAsync(
            long aggregateId,
            int count,
            CancellationToken cancellationToken = default)
        {
            return _context.Set<Event>().AsAsyncEnumerable().Reverse().Take(count);
        }

        public IAsyncEnumerable<Event> StreamAsync(
            long aggregateId,
            int startPosition = int.MinValue,
            int endPosition = int.MaxValue,
            CancellationToken cancellationToken = default)
        {
            return _context.Set<Event>().AsQueryable().Where(Matches).ToAsyncEnumerable();

            bool Matches(Event @event) =>
                @event.AggregateId == aggregateId &&
                @event.Position >= startPosition &&
                @event.Position <= endPosition;
        }
    }
}
