﻿using Common.General.SharedKernel;
using MassTransit;
using MassTransit.RabbitMqTransport;
using MassTransit.Util;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.RabbitMq
{
    public abstract class ComsumerService
    {
        private IBusControl _busControl;
        private readonly string _serviceQueueName;
        private readonly IConfiguration _configuration;

        protected ComsumerService(IConfiguration configuration, string serviceQueueName)
        {
            _serviceQueueName = serviceQueueName;
            _configuration = configuration;
        }

        public bool Start()
        {
            _busControl = Bus.Factory.CreateUsingRabbitMq(x =>
            {
                var username = _configuration.GetConnectionString(ApplicationConstants.MessageBusUsername);
                var password = _configuration.GetConnectionString(ApplicationConstants.MessageBusPassword);
                IRabbitMqHost host = x.Host(new Uri(_configuration.GetConnectionString(ApplicationConstants.MessageBusHost)), h =>
                {
                    if (!string.IsNullOrEmpty(username))
                    {
                        h.Username(_configuration.GetConnectionString(ApplicationConstants.MessageBusUsername));
                    }
                    if (!string.IsNullOrEmpty(password))
                    {
                        h.Password(_configuration.GetConnectionString(ApplicationConstants.MessageBusPassword));
                    }
                });

                x.ReceiveEndpoint(host, _serviceQueueName, Configure());
            });

            TaskUtil.Await(() => _busControl.StartAsync());

            return true;
        }

        public bool Stop()
        {
            _busControl?.Stop();

            return true;
        }

        public abstract Action<IRabbitMqReceiveEndpointConfigurator> Configure();
    }
}
