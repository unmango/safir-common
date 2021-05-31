using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Safir.Messaging;

namespace Safir.EventSourcing
{
    public static class AggregateExtensions
    {
        public static async IAsyncEnumerable<Event> SerializeAsync(
            this IAggregate aggregate,
            IEventSerializer serializer,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            foreach (var @event in aggregate.DequeueAllEvents())
            {
                yield return await serializer.SerializeAsync(@event, cancellationToken);
            }
        }

        public static async ValueTask<T> DeserializeAsync<T>(
            this IAsyncEnumerable<IEvent> events,
            IEventSerializer serializer,
            CancellationToken cancellationToken = default)
            where T : IAggregate, new()
        {
            
        }
    }
}
