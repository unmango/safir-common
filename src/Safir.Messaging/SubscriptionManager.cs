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
            foreach (var subscription in _subscriptions)
            {
                subscription.Dispose();
            }
            
            return Task.CompletedTask;
        }

        private void Subscribe(Type eventType, IEnumerable<IEventHandler> handlers)
        {
            var subscribe = typeof(EventBusExtensions).GetMethod(
                nameof(EventBusExtensions.Subscribe),
                BindingFlags.Public | BindingFlags.Static,
                null,
                new[] { typeof(IEventBus), typeof(IEventHandler<>).MakeGenericType(eventType) },
                null);

            if (subscribe == null)
            {
                _logger.LogError("Unable to retrieve EventBusExtensions.Subscribe method");
                return;
            }
            
            var closed = subscribe.MakeGenericMethod(eventType);

            foreach (var handler in handlers)
            {
                var subscription = closed.Invoke(null, new object[] { _eventBus, handler });
                
                _subscriptions.Add((IDisposable)subscription);
            }
        }
    }
}
