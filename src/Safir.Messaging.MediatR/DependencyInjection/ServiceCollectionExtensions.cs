using System.Reflection;
using JetBrains.Annotations;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Safir.Messaging.MediatR.DependencyInjection
{
    [UsedImplicitly]
    public static class ServiceCollectionExtensions
    {
        [UsedImplicitly]
        public static IServiceCollection AddMediatrAdapter(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetEntryAssembly());
            services.AddTransient(typeof(INotificationHandler<>), typeof(EventHandlerAdapter<,>));
            
            return services;
        }
    }
}
