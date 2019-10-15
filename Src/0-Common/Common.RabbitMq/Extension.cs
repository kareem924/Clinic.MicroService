using System;
using System.Linq;
using System.Reflection;
using Common.Commands;
using Common.Events;
using Common.Extensions.System;
using Common.General.Interfaces;
using Common.General.SharedKernel;
using MassTransit;
using MassTransit.Util;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common.RabbitMq
{
    public static class Extension
    {
        public static void AddRabbitMqMessageBus(this IServiceCollection services, IConfiguration configuration, Assembly assembly)
        {
            services.AddScoped<ICommandBus, CommandBus>();
            services.AddScoped<IEventBus, EventBus>();

            var allCommandHandler = assembly.GetTypes().Where(t =>
                t.IsClass &&
                !t.IsAbstract &&
                t.IsAssignableToGenericType(typeof(ICommandHandler<>)));
            foreach (var type in allCommandHandler)
            {
                var allInterfaces = type.GetInterfaces();
                var mainInterfaces = allInterfaces.Where(t => t.IsAssignableToGenericType(typeof(ICommandHandler<>)));
                foreach (var serviceType in mainInterfaces)
                {
                    services.AddScoped(serviceType, type);
                }
            }

            var allEventHandlers = assembly.GetTypes().Where(t =>
                t.IsClass &&
                !t.IsAbstract &&
                t.IsAssignableToGenericType(typeof(IEventHandler<>)));
            foreach (var type in allEventHandlers)
            {
                var allInterfaces = type.GetInterfaces();
                var mainInterfaces = allInterfaces.Where(t => t.IsAssignableToGenericType(typeof(IEventHandler<>)));
                foreach (var itype in mainInterfaces)
                {
                    services.AddScoped(itype, type);
                }
            }
            IBusControl bus = null;
            services.AddScoped(i =>
                {
                    if (bus == null)
                    {
                        bus = Bus.Factory.CreateUsingRabbitMq(x =>
                        {
                            var username = configuration.GetConnectionString(ApplicationConstants.MessageBusUsername);
                            var password = configuration.GetConnectionString(ApplicationConstants.MessageBusPassword);
                            var host = configuration.GetConnectionString(ApplicationConstants.MessageBusHost);
                            if (!string.IsNullOrEmpty(host))
                            {
                                x.Host(new Uri(host), h =>
                                {
                                    if (!string.IsNullOrEmpty(username))
                                    {
                                        h.Username(configuration.GetConnectionString(ApplicationConstants.MessageBusUsername));
                                    }
                                    if (!string.IsNullOrEmpty(password))
                                    {
                                        h.Password(configuration.GetConnectionString(ApplicationConstants.MessageBusPassword));
                                    }

                                });
                            }
                        });
                        TaskUtil.Await(() => bus.StartAsync());
                    }
                    return bus;
                });
            services.AddScoped<IMessageBus, MessageBus>();
        }
    }
}
