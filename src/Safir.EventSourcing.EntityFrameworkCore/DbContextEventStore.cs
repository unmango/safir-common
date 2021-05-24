using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Safir.Messaging;

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

        public Task AddAsync<T>(T @event, CancellationToken cancellationToken = default) where T : IEvent
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
            ulong startPosition = UInt64.MinValue,
            ulong endPosition = UInt64.MaxValue,
            CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
