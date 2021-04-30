using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Safir.Messaging
{
    internal sealed class SubscriptionManager : IHostedService
    {
        private readonly IEnumerable<IEventHandler> _handlers;
        private readonly IEventBus _eventBus;
        private readonly ILogger<SubscriptionManager> _logger;
        private readonly List<IDisposable> _subscriptions = new();

        public SubscriptionManager(
            IEnumerable<IEventHandler> handlers,
            IEventBus eventBus,
            ILogger<SubscriptionManager> logger)
        {
            _handlers = handlers ?? throw new ArgumentNullException(nameof(handlers));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // ReSharper disable once UseIsOperator.2
            var relevant = _handlers.Where(x => typeof(IEventHandler<>).IsInstanceOfType(x));
            var grouped = relevant.GroupBy(x => x.GetType().GetGenericArguments()[0]);
            
            foreach (var handlers in grouped)
            {
                Subscribe(handlers.Key, handlers);
            }
            
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private void Subscribe(Type eventType, IEnumerable<IEventHandler> handlers)
        {
            var getObservable = typeof(IEventBus).GetMethod(nameof(IEventBus.GetObservable));
            var subscribe = typeof(EventBusExtensions).GetMethod(nameof(EventBusExtensions.Subscribe));

            if (getObservable == null)
            {
                _logger.LogError("Unable to retrieve IEventBus.GetObservable method");
                return;
            }
            
            var closed = getObservable.MakeGenericMethod(eventType);

            foreach (var handler in handlers)
            {
                
            }
        }
    }
}
