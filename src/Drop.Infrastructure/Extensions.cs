using Drop.Core.Repositories;
using Drop.Infrastructure.Caching;
using Microsoft.Extensions.DependencyInjection;

namespace Drop.Infrastructure
{
    public static class Extensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddScoped<IParcelsRepository, InMemoryParcelsRepository>();
            
            return services;
        }
    }
}