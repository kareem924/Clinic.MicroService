using Common.Commands;
using Common.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RawRabbit;
using RawRabbit.Configuration;
using RawRabbit.Extensions.Client;
using RawRabbit.vNext;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
