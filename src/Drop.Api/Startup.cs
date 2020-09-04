using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Drop.Api.Services;
using Drop.Application;
using Drop.Application.Commands;
using Drop.Application.Services;
using Drop.Infrastructure;
using Drop.Infrastructure.Caching;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

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
            services.AddApplication();
            services.AddInfrastructure();
            services.AddControllers().AddNewtonsoftJson();
            services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc("v1", new OpenApiInfo {Title = "Drop API", Version = "v1"});
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(swagger =>
            {
                swagger.SwaggerEndpoint("/swagger/v1/swagger.json", "Drop API v1");
            });

            app.UseInfrastructure();

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

            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGet("/", async context =>
                {
                    var messenger = context.RequestServices.GetService<IMessenger>();
                    await context.Response.WriteAsync(messenger.GetMessage());
                });

                endpoints.MapGet("parcels/{parcelId:guid}", async context =>
                {
                    var parcelId = Guid.Parse(context.Request.RouteValues["parcelId"].ToString());
                    var parcelService = context.RequestServices.GetRequiredService<IParcelsService>();
                    var parcel = await parcelService.GetAsync(parcelId);
                    if (parcel is null)
                    {
                        context.Response.StatusCode = (int) HttpStatusCode.NotFound;
                        return;
                    }

                    context.Response.ContentType = "application/json";
                    // var json= JsonSerializer.Serialize(parcel);
                    var json = JsonConvert.SerializeObject(parcel);
                    await context.Response.WriteAsync(json);
                });

                endpoints.MapPost("parcels", async context =>
                {
                    var body = context.Request.Body;
                    if (body is null)
                    {
                        context.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                        return;
                    }

                    var json = await new StreamReader(body).ReadToEndAsync();
                    // var command = JsonSerializer.Deserialize<AddParcel>(json, new JsonSerializerOptions
                    // {
                    //     PropertyNameCaseInsensitive = true
                    // });

                    var command = JsonConvert.DeserializeObject<AddParcel>(json);
                    var parcelService = context.RequestServices.GetRequiredService<IParcelsService>();
                    await parcelService.AddAsync(command);

                    context.Response.Headers.Add(HttpResponseHeader.Location.ToString(), $"parcels/{command.Id}");
                    context.Response.StatusCode = 201;
                });
            });
        }
    }
}
