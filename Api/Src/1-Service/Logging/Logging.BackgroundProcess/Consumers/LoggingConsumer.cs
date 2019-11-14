using System.Threading.Tasks;
using Common.Events;
using Common.General.Repository;
using Common.RabbitMq;
using Logging.Core.Entities;
using MassTransit;

namespace Logging.BackgroundProcess.Consumers
{
    public class LoggingConsumer : BaseConsumer, IConsumer<WriteLogEvent>
    {
        private readonly IRepository<LogEntry> _logEntryRepository;

        public LoggingConsumer(IRepository<LogEntry> logEntryRepository)
        {
            _logEntryRepository = logEntryRepository;
        }

        public async Task Consume(ConsumeContext<WriteLogEvent> context)
        {
            context.Respond(new { Status = true });
            await _logEntryRepository.AddAsync(new LogEntry(
                context.Message.Level,
                context.Message.Thread,
                context.Message.Logger,
                context.Message.Message,
                context.Message.Data,
                context.Message.StackTrace,
                context.Message.ExceptionTypeName));
        }
    }
}
