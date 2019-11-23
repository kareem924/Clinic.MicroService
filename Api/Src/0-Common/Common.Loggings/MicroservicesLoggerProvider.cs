using Common.RabbitMq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Common.Loggings
{
    public class MicroservicesLoggerProvider : ILoggerProvider
    {
        private readonly IEventBus _messageBus;
        private readonly IConfiguration _configuration;

        public MicroservicesLoggerProvider(IEventBus messageBus, IConfiguration configuration)
        {
            _messageBus = messageBus;
            _configuration = configuration;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new MicroservicesLogging(categoryName, _configuration, _messageBus);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // do nothing now
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
