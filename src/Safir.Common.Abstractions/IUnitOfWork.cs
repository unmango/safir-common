using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Safir.Common
{
    [PublicAPI]
    public interface IUnitOfWork : IDisposable, IAsyncDisposable
    {
        void SaveChanges();

        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
