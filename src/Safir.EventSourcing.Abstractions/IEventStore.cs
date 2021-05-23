using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Safir.EventSourcing
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public interface IEventStore
    {
        Task AddAsync(Event @event, CancellationToken cancellationToken = default);

        Task<Event> GetAsync(long id, CancellationToken cancellationToken = default);

        IAsyncEnumerable<Event> GetStreamAsync(
            long aggregateId,
            ulong startPosition = ulong.MinValue,
            ulong endPosition = ulong.MaxValue,
            CancellationToken cancellationToken = default);
    }
}
