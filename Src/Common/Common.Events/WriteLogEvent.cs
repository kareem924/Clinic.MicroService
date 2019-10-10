using System;

namespace Common.Events
{
    [MessageBusRoute(EventRouteConstants.LoggingService)]
    public class WriteLogEvent : IEvent
    {
        public DateTime Date { get { return DateTime.Now; } }
        public string Level { get; set; }
        public string Thread { get; set; }
        public string Logger { get; set; }
        public string Message { get; set; }
        public string Data { get; set; }
        public string StackTrace { get; set; }
        public string ExceptionTypeName { get; set; }
    }
}
