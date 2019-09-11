using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Events;
using Common.RabbitMq;
using MassTransit;

namespace Logging.BackgroundProcess.Consumers
{
    public class LoggingConsumer : BaseConsumer, IConsumer<WriteLogEvent>
    {
       

        public LoggingConsumer() : base()
        {
        }

        public Task Consume(ConsumeContext<WriteLogEvent> context)
        {
            context.Respond(new { Status = true });
            return Task.CompletedTask;
        }
    }
}
