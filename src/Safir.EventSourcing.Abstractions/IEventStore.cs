using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Safir.Messaging;

namespace Safir.EventSourcing
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public interface IEventStore
    {
        Task AddAsync<T>(T @event, CancellationToken cancellationToken = default)
            where T : IEvent;

        Task<IEvent> GetAsync(long id, CancellationToken cancellationToken = default);

        Task<T> GetAsync<T>(long id, CancellationToken cancellationToken = default)
            where T : IEvent;

        IAsyncEnumerable<IEvent> GetStreamAsync(
            long aggregateId,
            ulong startPosition = ulong.MinValue,
            ulong endPosition = ulong.MaxValue,
            CancellationToken cancellationToken = default);
    }
}
