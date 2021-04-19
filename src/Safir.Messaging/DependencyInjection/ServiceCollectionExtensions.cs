using Microsoft.Extensions.DependencyInjection;
using Safir.Common.ConnectionPool.DependencyInjection;
using Safir.Messaging.Configuration;
using StackExchange.Redis;

namespace Safir.Messaging.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSafirMessaging(this IServiceCollection services)
        {
            services.AddLogging();
            services.AddOptions<RedisOptions>();

            services.AddConnectionPool<IConnectionMultiplexer, CreateRedisConnection>();
            services.AddTransient<IEventBus, RedisEventBus>();
            
            return services;
        }
    }
}
