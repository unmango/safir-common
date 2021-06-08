using System;
using System.Threading;
using System.Threading.Tasks;

namespace Safir.EventSourcing
{
    public static class SnapshotStoreExtensions
    {
        public static ValueTask<T> FindAsync<T>(
            this ISnapshotStore store,
            Guid id,
            int maxVersion,
            CancellationToken cancellationToken = default)
            where T : IAggregate
        {
            return store.FindAsync<T, Guid>(id, maxVersion, cancellationToken);
        }
    }
}
