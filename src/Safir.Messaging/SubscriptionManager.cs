using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
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
            _logger.LogDebug("Starting subscription manager");
            foreach (var group in _handlers.GroupByEvent())
            {
                _logger.LogTrace("Subscribing handlers for event type {Type}", group.Key);
                var subscriptions = _eventBus.Subscribe(group.Key, group)
                    .Select(x => x.IfFail(e => {
                        _logger.LogError(e, "Handler failed to subscribe");
                        return Disposable.Empty;
                    }));
                
                _subscriptions.AddRange(subscriptions);
            }
            
            _logger.LogTrace("Exiting StartAsync");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug("Stopping subscription manager");
            foreach (var subscription in _subscriptions)
            {
                subscription.Dispose();
            }
            
            _logger.LogTrace("Exiting StopAsync");
            return Task.CompletedTask;
        }
    }
}
