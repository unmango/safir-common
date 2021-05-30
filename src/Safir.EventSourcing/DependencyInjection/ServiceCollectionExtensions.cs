using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Safir.Common;

namespace Safir.EventSourcing.DependencyInjection
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEventSourcing(this IServiceCollection services)
        {
            services.AddLogging();
            
            services.AddTransient<ISerializer, DefaultSerializer>();
            services.AddSingleton<IEventMetadataProvider, DefaultEventMetadataProvider>();
            
            return services;
        }
    }
}
