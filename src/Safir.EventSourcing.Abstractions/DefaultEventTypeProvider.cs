using JetBrains.Annotations;
using Safir.Messaging;

namespace Safir.EventSourcing
{
    [UsedImplicitly]
    public sealed class DefaultEventTypeProvider : IEventTypeProvider
    {
        public string GetType<T>(T @event) where T : IEvent => typeof(T).Name;
    }
}
