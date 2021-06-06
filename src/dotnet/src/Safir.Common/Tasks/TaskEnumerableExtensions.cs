using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Safir.Common.Tasks
{
    [PublicAPI]
    public static class TaskEnumerableExtensions
    {
        public static async IAsyncEnumerable<T> AsAsyncEnumerable<T>(
            this Task<IEnumerable<T>> task,
            [EnumeratorCancellation]
            CancellationToken cancellationToken = default)
        {
            foreach (var item in await task)
            {
                cancellationToken.ThrowIfCancellationRequested();
                yield return item;
            }
        }

        public static async IAsyncEnumerable<T> AsAsyncEnumerable<T>(
            this Task<IReadOnlyList<T>> task,
            [EnumeratorCancellation]
            CancellationToken cancellationToken = default)
        {
            foreach (var item in await task)
            {
                cancellationToken.ThrowIfCancellationRequested();
                yield return item;
            }
        }
        
        public static TaskAwaiter<T[]> GetAwaiter<T>(this IEnumerable<Task<T>> enumerable)
        {
            return Task.WhenAll(enumerable).GetAwaiter();
        }
        
        public static TaskAwaiter<T[]> GetAwaiter<T>(this IEnumerable<ValueTask<T>> enumerable)
        {
            return Task.WhenAll(enumerable.Select(x => x.AsTask())).GetAwaiter();
        }
    }
}
