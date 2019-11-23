using Common.RabbitMq;

namespace Common.Loggings
{
   public class WriteLogEvent:IntegrationEvent
    {
        public string Level { get; set; }
        public string Logger { get; set; }
        public string Thread { get; set; }
        public string Message { get; set; }
        public string Data { get; set; }
        public string StackTrace { get; set; }
        public string ExceptionTypeName { get; set; }
    }
}
