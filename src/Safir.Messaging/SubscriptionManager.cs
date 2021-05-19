using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Safir.Messaging
{
    internal sealed class SubscriptionManager : IHostedService
    {
        private readonly IServiceProvider _services;
        private readonly IEventBus _eventBus;
        private readonly ILogger<SubscriptionManager> _logger;
        private readonly List<IDisposable> _subscriptions = new();

        public SubscriptionManager(
            IServiceProvider services,
            IEventBus eventBus,
            ILogger<SubscriptionManager> logger)
        {
            _services = services ?? throw new ArgumentNullException(nameof(services));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug("Starting subscription manager");
            using var scope = _services.CreateScope();
            var handlers = scope.ServiceProvider.GetRequiredService<IEnumerable<IEventHandler>>();

            foreach (var type in handlers.GetEventTypes())
            {
                
            }
            
            // foreach (var group in handlers.GroupByEvent())
            // {
            //     _logger.LogTrace("Subscribing handlers for event type {Type}", group.Key);
            //     foreach (var handler in group)
            //     {
            //         try
            //         {
            //             var subscription = _eventBus.SubscribeRetry(group.Key, handler, e => {
            //                 _logger.LogError(e, "Exception in handler");
            //             });
            //
            //             _subscriptions.Add(subscription);
            //         }
            //         catch (Exception e)
            //         {
            //             _logger.LogError(e, "Exception while subscribing handler");
            //         }
            //     }
            // }

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
