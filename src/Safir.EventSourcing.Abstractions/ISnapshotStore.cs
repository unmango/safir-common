using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Safir.EventSourcing
{
    [PublicAPI]
    public interface ISnapshotStore
    {
        Task AddAsync<TAggregate, TId>(TAggregate aggregate, CancellationToken cancellationToken = default)
            where TAggregate : IAggregate<TId>;

        ValueTask<TAggregate> FindAsync<TAggregate, TId>(
            TId aggregateId,
            int maxVersion,
            CancellationToken cancellationToken = default)
            where TAggregate : IAggregate<TId>;
    }
}
