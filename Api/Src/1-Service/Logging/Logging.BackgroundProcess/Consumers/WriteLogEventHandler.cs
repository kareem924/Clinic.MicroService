using System.Threading.Tasks;
using Common.General.Repository;
using Common.RabbitMq;
using Logging.Core.Entities;

namespace Logging.BackgroundProcess.Consumers
{
    public class WriteLogEventHandler : IIntegrationEventHandler<WriteLogEvent>
    {
        private readonly IRepository<LogEntry> _loggingRepository;
        public WriteLogEventHandler(IRepository<LogEntry> loggingRepository)
        {
            _loggingRepository = loggingRepository;
        }
        public async Task Handle(WriteLogEvent @event)
        {
            await _loggingRepository.AddAsync(new LogEntry(
                @event.Level,
                @event.Thread,
                @event.Logger,
                @event.Message,
                @event.Data,
                @event.StackTrace,
                @event.StackTrace));
        }
    }
}
