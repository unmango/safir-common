using Microsoft.Extensions.DependencyInjection;

namespace Safir.Messaging.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSafirMessaging(this IServiceCollection services)
        {
            services.AddLogging();

            services.AddTransient<IEventBus, RedisEventBus>();
            
            return services;
        }
    }
}
