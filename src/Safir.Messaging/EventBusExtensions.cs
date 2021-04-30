using System;

namespace Safir.Messaging
{
    public static class EventBusExtensions
    {
        public static IDisposable Subscribe<T>(this IEventBus bus, Action<T> callback)
            where T : IEvent
        {
            return bus.GetObservable<T>().Subscribe(callback);
        }
    }
}
