using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Safir.Common.ConnectionPool
{
    internal sealed class DefaultConnectionPool<T> : IConnectionPool<T>, IAsyncDisposable
    {
        private readonly IOptions<ConnectionPoolOptions<T>> _configuration;
        private readonly ConcurrentBag<T> _connections = new();
        private readonly Func<IEnumerable<T>, T> _selector;
        private volatile int _toSkip = 0;

        public DefaultConnectionPool(IOptions<ConnectionPoolOptions<T>> configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _selector = configuration.Value.Selector ?? DefaultSelector;
        }

        private int PoolSize => _configuration.Value.PoolSize;
        
        public ValueTask<T> GetConnectionAsync(CancellationToken cancellationToken = default)
        {
            if (_connections.Count == PoolSize) return new(_selector(_connections));

            return new(Task.Run(async () => {
                var connection = await ConnectAsync(cancellationToken);
                _connections.Add(connection);
                return connection;
            }, cancellationToken));
        }

        public void Dispose() => DisposeAsync().GetAwaiter().GetResult();

        public ValueTask DisposeAsync()
        {
            if (_connections.IsEmpty) return new();

            return new(Task.WhenAll(_connections.Select(DisposeConnection)));
        }

        private Task<T> ConnectAsync(CancellationToken cancellationToken)
        {
            return _configuration.Value.CreateConnection(cancellationToken);
        }

        private Task DisposeConnection(T connection)
        {
            return _configuration.Value.DisposeConnection(connection);
        }

        private T DefaultSelector(IEnumerable<T> connections)
        {
            _toSkip = (_toSkip + 1) % _connections.Count;
            return connections.Skip(_toSkip).First();
        }
    }
}
