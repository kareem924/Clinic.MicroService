using System;

namespace Logging.Core.Entities
{
    public class LogEntry
    {
        public DateTime Date => DateTime.Now;

        public string Level { get; private set; }

        public string Thread { get; private set; }

        public string Logger { get; private set; }

        public string Message { get; private set; }

        public string Data { get; private set; }

        public string StackTrace { get; private set; }

        public string ExceptionTypeName { get; private set; }

        public LogEntry()
        {

        }

        public LogEntry(
            string level,
            string thread,
            string logger,
            string message,
            string data,
            string stackTrace,
            string exceptionTypeName
            )
        {
            Level = level;
            Thread = thread;
            Logger = logger;
            Message = message;
            Data = data;
            StackTrace = stackTrace;
            ExceptionTypeName = exceptionTypeName;
        }
    }

}
