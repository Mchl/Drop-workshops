using Drop.Core.Repositories;
using Drop.Infrastructure.Caching;
using Drop.Infrastructure.Mongo;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Drop.Infrastructure
{
    public static class Extensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            // services.AddScoped<IParcelsRepository, InMemoryParcelsRepository>();
            services.AddMongo();
            services.AddScoped<ErrorHandlerMiddleware>();
            services.AddMemoryCache();
            
            return services;
        }

        public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
        {
            app.UseMiddleware<ErrorHandlerMiddleware>();

            return app;
        }
    }
}