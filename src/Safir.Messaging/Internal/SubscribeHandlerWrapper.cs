using System;

namespace Safir.Messaging.Internal
{
    internal sealed class SubscribeHandlerWrapper<T> : ISubscribeHandlerWrapper
        where T : IEvent
    {
        public IDisposable Subscribe(IEventBus bus, IEventHandler handler)
        {
            return SubscribeSafe(bus, handler, _ => { });
        }

        public IDisposable SubscribeSafe(IEventBus bus, IEventHandler handler, Action<Exception> onError)
        {
            if (handler is not IEventHandler<T> typed)
            {
                throw new InvalidOperationException("Incorrect handler type");
            }

            return bus.SubscribeSafe(typed, onError);
        }
    }
}
