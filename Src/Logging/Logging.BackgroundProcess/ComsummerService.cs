using System;
using Common.Events;
using Common.RabbitMq;
using Logging.BackgroundProcess.Consumers;
using MassTransit;
using MassTransit.RabbitMqTransport;
using Microsoft.Extensions.Configuration;

namespace Logging.BackgroundProcess
{
    public class ConsumerLoggingService : ConsumerService
    {

        public ConsumerLoggingService(IConfiguration configuration) :
            base(configuration, EventRouteConstants.LoggingService)
        {
        }

        public override Action<IRabbitMqReceiveEndpointConfigurator> Configure()
        {
            return e =>
            {
                e.Consumer(() => new LoggingConsumer());
            };
        }
    }
}
