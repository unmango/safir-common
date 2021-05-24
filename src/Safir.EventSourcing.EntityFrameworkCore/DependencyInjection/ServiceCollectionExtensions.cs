using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Safir.EventSourcing.EntityFrameworkCore.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEventDbContext<T>(this IServiceCollection services)
            where T : DbContext
        {
            return services.AddScoped<IEventStore, DbContextEventStore<T>>();
        }
    }
}
