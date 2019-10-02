using System;
using System.Collections.Generic;
using System.Text;
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
        public static void AddRabbitMqMessageBus(this IServiceCollection services, IConfiguration configuration)
        {
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
