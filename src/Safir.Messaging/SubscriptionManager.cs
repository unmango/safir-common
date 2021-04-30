using System;
using System.Collections.Generic;
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
            throw new NotImplementedException();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
