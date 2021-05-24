using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Safir.EventSourcing.DependencyInjection
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEventSourcing(this IServiceCollection services)
        {
            services.AddSingleton<IEventTypeProvider, DefaultEventTypeProvider>();
            
            return services;
        }
    }
}
