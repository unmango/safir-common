using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Safir.Common
{
    [PublicAPI]
    public abstract class SaveOnDisposeUnitOfWork : IUnitOfWork
    {
        public virtual void Dispose() => SaveChanges();

        public virtual ValueTask DisposeAsync() => new(SaveChangesAsync());

        public abstract void SaveChanges();

        public abstract Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
