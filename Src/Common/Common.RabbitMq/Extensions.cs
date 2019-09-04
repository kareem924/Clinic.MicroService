using Common.Commands;
using Common.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RawRabbit;
using RawRabbit.Common;
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
        public static ISubscription SubscribeToCommand<TCommand>(this RawRabbit.IBusClient bus,
       ICommandHandler<TCommand> handler, string name = null) where TCommand : ICommand
      => bus.SubscribeAsync<TCommand>(async (msg, context) => await handler.HandleAsync(msg),
       cfg => cfg.WithQueue(q => q.WithName(GetExchangeName<TCommand>(name))));

        public static ISubscription SubscribeToEvent<TEvent>(this RawRabbit.IBusClient bus,
            IEventHandler<TEvent> handler, string name = null) where TEvent : IEvent
        => bus.SubscribeAsync<TEvent>(async (msg, context) => await handler.HandleAsync(msg),
            cfg => cfg.WithQueue(q => q.WithName(GetExchangeName<TEvent>(name))));

        private static string GetExchangeName<T>(string name = null)
            => string.IsNullOrWhiteSpace(name)
                ? $"{Assembly.GetEntryAssembly().GetName()}/{typeof(T).Name}"
                : $"{name}/{typeof(T).Name}";

        public static void AddRabbitMq(this IServiceCollection services, IConfiguration configuration)
        {
           
            var options = new RabbitMqOptions();
            var section = configuration.GetSection("rabbitmq");
            section.Bind(options);
            var busClient = BusClientFactory.CreateDefault(options);
        }
    }
}
