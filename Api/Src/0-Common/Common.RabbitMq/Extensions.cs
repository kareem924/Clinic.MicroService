using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Common.RabbitMq
{
    public static class Extensions
    {
        private static readonly Lazy<Type[]> _eventHandlerTypes = new Lazy<Type[]>(GetHandlerTypes);

        public static IServiceCollection AddIntegrationSupport(
            this IServiceCollection services)
        {
            services.AddSingleton<IRabbitMQPersistentConnection, DefaultRabbitMQPersistentConnection>();
            services.AddSingleton<IEventBus, EventBusRabbitMQ>();
            services.AddSingleton<IEventBusSubscriptionsManager, EventBusRabbitMQ>();
            return services;
        }

        public static IApplicationBuilder AddIntegrationSupport(this IApplicationBuilder applicationBuilder)
        {
            var subscriptionsManager = applicationBuilder.ApplicationServices.GetRequiredService<IEventBusSubscriptionsManager>();
            _eventHandlerTypes.Value
                .SelectMany(type => type.GetInterfaces()
                .Where(@interface => @interface.IsGenericType && @interface.GetGenericTypeDefinition() == typeof(IIntegrationEventHandler<>))
                    .Select(@interface => @interface.GetGenericArguments()[0])
                    .Distinct())
                .ToList()
                .ForEach(eventType => subscriptionsManager.AddSubscription(eventType));
            return applicationBuilder;
        }

        private static Type[] GetHandlerTypes()
        {
            //TODO: 
             return AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                .Where(IsIntegrationEventHandler).ToArray();
        }

        private static object Select(Func<object, object> p)
        {
            throw new NotImplementedException();
        }

        private static bool IsIntegrationEventHandler(Type type)
        {
            return
                 type.IsClass && !type.IsAbstract && !type.ContainsGenericParameters &&
                 type.GetInterfaces().Any(interfaceType => interfaceType.IsConstructedGenericType &&
                 interfaceType.GetGenericTypeDefinition() == typeof(IIntegrationEventHandler<>));
        }
    }
}
