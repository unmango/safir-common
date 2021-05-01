using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
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

        public static IDisposable Subscribe<T>(this IEventBus bus, IEventHandler<T> handler)
            where T : IEvent
        {
            return bus.GetObservable<T>().SelectMany(HandleAsync).Subscribe();

            async Task<Unit> HandleAsync(T message, CancellationToken cancellationToken)
            {
                await handler.HandleAsync(message, cancellationToken);
                return Unit.Default;
            }
        }
    }
}
