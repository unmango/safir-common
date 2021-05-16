using System;
using System.Collections.Generic;
using System.ComponentModel;
using LanguageExt.Common;

namespace Safir.Messaging.Internal
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface ISubscribeHandlerWrapper
    {
        IDisposable Subscribe(IEventBus bus, IEventHandler handler);
    }
}
