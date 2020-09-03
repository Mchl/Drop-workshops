using Drop.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Drop.Application
{
    public static class Extensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IParcelsService, ParcelsService>();
            
            return services;
        }
    }
}