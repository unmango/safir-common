using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Safir.Messaging
{
    internal sealed class RedisEventBus : IEventBus
    {
        private readonly ILogger<RedisEventBus> _logger;

        public RedisEventBus(ILogger<RedisEventBus> logger)
        {
            _logger = logger;
        }
        
        public IObservable<T> GetObservable<T>() where T : IEvent
        {
            throw new NotImplementedException();
        }

        public Task PublishAsync<T>(T message) where T : IEvent
        {
            throw new NotImplementedException();
        }
    }
}
