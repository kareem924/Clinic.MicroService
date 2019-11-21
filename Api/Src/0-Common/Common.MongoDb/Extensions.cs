using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Common.MongoDb
{
    public static class Extensions
    {
        public static void AddMongoDB(this IServiceCollection services, IConfiguration configuration)
        {
            var mongoDbConfig = configuration.GetSection(nameof(MongoDbConfig));
            services.Configure<MongoDbConfig>(mongoDbConfig);
            services.AddSingleton<MongoClient>(c =>
            {
                var options = c.GetService<IOptions<MongoDbConfig>>();

                return new MongoClient(options.Value.ConnectionString);
            });
            services.AddScoped<IMongoDatabase>(c =>
            {
                var options = c.GetService<IOptions<MongoDbConfig>>();
                var client = c.GetService<MongoClient>();

                return client.GetDatabase(options.Value.Database);
            });
            services.AddScoped<IDatabaseInitializer, MongoInitializer>();
            services.AddScoped<IDatabaseSeeder, MongoSeeder>();
        }


    }
}
