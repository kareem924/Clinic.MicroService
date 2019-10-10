using System;
using Common.Events;
using Common.RabbitMq;
using Logging.BackgroundProcess.Consumers;
using MassTransit;
using MassTransit.RabbitMqTransport;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Logging.BackgroundProcess
{
    public class ConsumerLoggingService : ConsumerService
    {

        public ConsumerLoggingService(IConfiguration configuration, ILoggerFactory logger) :
            base(configuration, EventRouteConstants.LoggingService, logger)
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
