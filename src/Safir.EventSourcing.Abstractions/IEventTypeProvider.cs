using JetBrains.Annotations;
using Safir.Messaging;

namespace Safir.EventSourcing
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public interface IEventTypeProvider
    {
        string GetType<T>(T @event) where T : IEvent;
    }
}
