using System;
using JetBrains.Annotations;

namespace Safir.Messaging
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public static class EventBusExtensions
    {
        public static IDisposable Subscribe<T>(this IEventBus bus, Action<T> callback)
            where T : IEvent
        {
            return bus.GetObservable<T>().Subscribe(callback);
        }
    }
}
