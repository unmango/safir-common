using System;
using System.Collections.Generic;
using LanguageExt.Common;

namespace Safir.Messaging.Internal
{
    internal interface ISubscribeHandlerWrapper
    {
        IDisposable Subscribe(IEventBus bus, IEventHandler handler);
        
        IEnumerable<Result<IDisposable>> Subscribe(IEventBus bus, IEnumerable<IEventHandler> handlers);
    }
}
