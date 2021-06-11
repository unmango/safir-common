using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Safir.Messaging;

namespace Safir.EventSourcing
{
    [PublicAPI]
    public static class EventSerializerExtensions
    {
        public static async ValueTask<Event> SerializeAsync(
            this IEventSerializer serializer,
            Guid aggregateId,
            IEvent @event,
            CancellationToken cancellationToken = default)
        {
            return (Event)await serializer.SerializeAsync<Guid, Guid>(aggregateId, @event, cancellationToken);
        }

        public static async ValueTask<Event<T>> SerializeAsync<T>(
            this IEventSerializer serializer,
            T aggregateId,
            IEvent @event,
            CancellationToken cancellationToken = default)
        {
            return (Event<T>)await serializer.SerializeAsync<T, Guid>(aggregateId, @event, cancellationToken);
        }

        public static ValueTask<IEvent> DeserializeAsync(
            this IEventSerializer serializer,
            Event @event,
            CancellationToken cancellationToken = default)
        {
            return serializer.DeserializeAsync(@event, cancellationToken);
        }

        public static ValueTask<IEvent> DeserializeAsync<T>(
            this IEventSerializer serializer,
            Event<T> @event,
            CancellationToken cancellationToken = default)
        {
            return serializer.DeserializeAsync(@event, cancellationToken);
        }

        public static ValueTask<T> DeserializeAsync<T>(
            this IEventSerializer serializer,
            Event @event,
            CancellationToken cancellationToken = default)
            where T : IEvent
        {
            return serializer.DeserializeAsync<Guid, Guid, T>(@event, cancellationToken);
        }

        public static ValueTask<T> DeserializeAsync<TAggregateId, T>(
            this IEventSerializer serializer,
            Event<TAggregateId> @event,
            CancellationToken cancellationToken = default)
            where T : IEvent
        {
            return serializer.DeserializeAsync<TAggregateId, Guid, T>(@event, cancellationToken);
        }
    }
}
