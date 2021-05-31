using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Safir.EventSourcing
{
    [PublicAPI]
    public interface IAggregateStore
    {
        Task StoreAsync<T>(T aggregate, CancellationToken cancellationToken = default)
            where T : IAggregate;

        Task SnapshotAsync<T>(T aggregate, CancellationToken cancellationToken = default)
            where T : IAggregate;

        Task<T> GetAsync<T>(long id, CancellationToken cancellationToken = default)
            where T : IAggregate;

        Task<T> GetAsync<T>(long id, int version, CancellationToken cancellationToken = default)
            where T : IAggregate;
    }
}
