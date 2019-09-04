using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RawRabbit.vNext;

namespace Common.RabbitMq
{
    public static class Extensions
    {
       

        public static void AddRabbitMq(this IServiceCollection services, IConfiguration configuration)
        {
            var options = new RabbitMqOptions();
            var section = configuration.GetSection("rabbitmq");
            section.Bind(options);
            var busClient = BusClientFactory.CreateDefault(options);
        }
    }
}
