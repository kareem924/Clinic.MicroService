using Common.General.SharedKernel;
using MassTransit;
using MassTransit.RabbitMqTransport;
using MassTransit.Util;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Common.RabbitMq
{
    public abstract class ConsumerService
    {
        private IBusControl _busControl;
        private readonly string _serviceQueueName;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ConsumerService> _logger;

        protected ConsumerService(IConfiguration configuration, string serviceQueueName, ILoggerFactory logger)
        {
            _serviceQueueName = serviceQueueName;
            _configuration = configuration;
            _logger = logger.CreateLogger<ConsumerService>();
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
            var policy = RetryPolicy.Handle<SocketException>()
                  .Or<BrokerUnreachableException>()
                  .WaitAndRetry(5, retryAttempt => TimeSpan.FromSeconds(
                      Math.Pow(2, retryAttempt)), (ex, time) =>
                      {
                          _logger.LogWarning(
                                ex,
                                "RabbitMQ Client could not connect after {TimeOut}s ({ExceptionMessage})",
                                $"{time.TotalSeconds:n1}", ex.Message);
                      }
              );
            policy.Execute(() =>
            {
                TaskUtil.Await(() => _busControl.StartAsync());
            });
            
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
