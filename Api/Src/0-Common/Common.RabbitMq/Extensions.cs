using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;


namespace Common.RabbitMq
{
    public static class Extensions
    {
        //private static readonly Lazy<Type[]> EventHandlerTypes = new Lazy<Type[]>(GetHandlerTypes);

        public static IServiceCollection AddIntegrationSupport(
            this IServiceCollection services)
        {
            services.AddSingleton<IRabbitMQPersistentConnection, DefaultRabbitMQPersistentConnection>();
            services.AddSingleton<IEventBus, EventBusRabbitMQ>();
            services.AddSingleton<IEventBusSubscriptionsManager, EventBusRabbitMQ>();
            return services;
        }

        public static IApplicationBuilder AddIntegrationSupport(this IApplicationBuilder applicationBuilder,Assembly assembly)
        {
            var subscriptionsManager = applicationBuilder.ApplicationServices.GetRequiredService<IEventBusSubscriptionsManager>();
            GetHandlerTypes(assembly)
                .SelectMany(type => type.GetInterfaces()
                .Where(@interface => @interface.IsGenericType && @interface.GetGenericTypeDefinition() == typeof(IIntegrationEventHandler<>))
                    .Select(@interface => @interface.GetGenericArguments()[0])
                    .Distinct())
                .ToList()
                .ForEach(eventType => subscriptionsManager.AddSubscription(eventType));
            return applicationBuilder;
        }

        private static Type[] GetHandlerTypes(Assembly assembly)
        {
            return assembly.GetTypes().Where(IsIntegrationEventHandler).ToArray();
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
