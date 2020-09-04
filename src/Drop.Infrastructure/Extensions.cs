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
            services.AddMongo();
            services.AddScoped<ErrorHandlerMiddleware>();
            services.AddMemoryCache();
            services.AddScoped<IParcelsRepository, InMemoryParcelsRepository>();
            
            return services;
        }

        public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
        {
            app.UseMiddleware<ErrorHandlerMiddleware>();

            return app;
        }
    }
}