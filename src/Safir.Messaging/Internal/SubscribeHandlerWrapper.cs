using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;
using LanguageExt.Common;

namespace Safir.Messaging.Internal
{
    internal sealed class SubscribeHandlerWrapper<T> : ISubscribeHandlerWrapper
        where T : IEvent
    {
        public IEnumerable<Result<IDisposable>> Subscribe(IEventBus bus, IEnumerable<IEventHandler> handlers)
        {
            return handlers.Cast<IEventHandler<T>>().Select(x => Prelude.Try(() => bus.Subscribe(x))());
        }
    }
}
