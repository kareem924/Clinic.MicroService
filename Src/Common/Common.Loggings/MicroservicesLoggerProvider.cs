using Common.General.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Loggings
{
    public class MicroservicesLoggerProvider : ILoggerProvider
    {
        private readonly IMessageBus _messageBus;
        private readonly IConfiguration Configuration;

        public MicroservicesLoggerProvider(IMessageBus messageBus, IConfiguration configuration)
        {
            _messageBus = messageBus;
            Configuration = configuration;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new MicroservicesLogging(categoryName, Configuration, _messageBus);
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
