using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Safir.EventSourcing.Tests
{
    public class EmptyAsyncEnumerable<T> : IAsyncEnumerable<T>
    {
        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = new CancellationToken())
        {
            return new EmptyEnumerator();
        }

        private class EmptyEnumerator : IAsyncEnumerator<T>
        {
            public ValueTask DisposeAsync() => new();

            public ValueTask<bool> MoveNextAsync() => new(false);

            public T Current => throw new InvalidOperationException("Enumeration is empty");
        }
    }
}
