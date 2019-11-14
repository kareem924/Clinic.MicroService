using System;
using Common.Events;
using Common.General.Repository;
using Common.RabbitMq;
using Logging.BackgroundProcess.Consumers;
using Logging.Core.Entities;
using MassTransit;
using MassTransit.RabbitMqTransport;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Logging.BackgroundProcess
{
    public class ConsumerLoggingService : ConsumerService
    {
        private readonly IRepository<LogEntry> _logEntryRepository;

        public ConsumerLoggingService(IConfiguration configuration, ILoggerFactory logger, IRepository<LogEntry> logEntryRepository) :
            base(configuration, EventRouteConstants.LoggingService, logger)
        {
            _logEntryRepository = logEntryRepository;
        }

        public override Action<IRabbitMqReceiveEndpointConfigurator> Configure()
        {
            return e =>
            {
                e.Consumer(() => new LoggingConsumer(_logEntryRepository));
            };
        }
    }
}
