using Drop.Core.Repositories;
using Drop.Core.ValueObjects;
using Drop.Infrastructure.Mongo.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace Drop.Infrastructure.Mongo
{
    internal static class Extensions
    {
        public static IServiceCollection AddMongo(this IServiceCollection services)
        {
            services.AddScoped<IParcelsRepository, MongoParcelsRepository>();
            IConfiguration configuration;
            using (var serviceProvider = services.BuildServiceProvider())
            {
                configuration = serviceProvider.GetService<IConfiguration>();
            }

            var section = configuration.GetSection("mongo");
            services.Configure<MongoOptions>(section);
            var mongoOptions = new MongoOptions();
            section.Bind(mongoOptions);

            services.AddSingleton(mongoOptions);
            services.AddSingleton<IMongoClient>(sp =>
            {
                var options = sp.GetService<MongoOptions>();
                return new MongoClient(options.ConnectionString);
            });

            services.AddTransient(sp =>
            {
                var options = sp.GetService<IOptions<MongoOptions>>().Value;
                var client = sp.GetService<IMongoClient>();
                return client.GetDatabase(options.Database);
            });

            ConventionRegistry.Register("drop", new ConventionPack
            {
                new EnumRepresentationConvention(BsonType.String),
                new CamelCaseElementNameConvention(),
                new IgnoreExtraElementsConvention(true),
            }, _ => true);

            return services;
        }
    }
}