using System;
using System.Net;
using System.Threading.Tasks;
using Drop.Api.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Drop.Api
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IMessenger, Messenger>();
            services.Configure<ApiOptions>(_configuration.GetSection("api"));
            services.AddScoped<DummyMiddleware>();
            services.AddScoped<ErrorHandlerMiddleware>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseMiddleware<ErrorHandlerMiddleware>();

            app.Use((ctx, next) =>
            {
                Console.WriteLine("I'm the first middleware.");
                return next();
            });

            app.Use(async (ctx, next) =>
            {
                Console.WriteLine("I'm the second middleware.");
                await next();
            });

            app.UseMiddleware<DummyMiddleware>();

            app.Use(async (ctx, next) =>
            {
                if (ctx.Request.Query.TryGetValue("token", out var token) && token == "secret")
                {
                    await ctx.Response.WriteAsync("Secret");
                    return;
                }

                await next();
            });

            
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    var messenger = context.RequestServices.GetService<IMessenger>();
                    await context.Response.WriteAsync(messenger.GetMessage());
                });

                endpoints.MapGet("parcels/{parcelId:guid}", async context =>
                {
                    var parcelId = Guid.Parse(context.Request.RouteValues["parcelId"].ToString());
                    if (parcelId == Guid.Empty)
                    {
                        context.Response.StatusCode = (int) HttpStatusCode.NotFound;
                        return;
                    }

                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync("{}");
                });
                
                endpoints.MapPost("parcels", context =>
                {
                    var parcelId = Guid.NewGuid();
                    context.Response.Headers.Add(HttpResponseHeader.Location.ToString(), $"parcels/{parcelId}");
                    context.Response.StatusCode = 201;
                    
                    return Task.CompletedTask;
                });
            });
        }
    }
}
