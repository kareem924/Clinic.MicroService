using Common.Events;
using Common.General.SharedKernel;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;

namespace Common.RabbitMq
{
    public class BaseConsumer
    {
        protected void WriteErrorLog<T>(ConsumeContext<T> context, IConfiguration Configuration, string message, object data) where T : class
        {
            WriteLog(context, Configuration, LogLevel.Error, message, data);
        }

        protected void WriteInformationLog<T>(ConsumeContext<T> context, IConfiguration Configuration, string message) where T : class
        {
            WriteLog(context, Configuration, LogLevel.Information, message);
        }

        private void WriteLog<T>(
            ConsumeContext<T> context,
            IConfiguration Configuration,
            LogLevel logLevel,
            string message,
            object data = null) where T : class
        {
            var sendEndPoint = context.GetSendEndpoint(
                new Uri($"{Configuration.GetConnectionString(ApplicationConstants.MessageBusHost)}/" +
                $"logging_service")).Result;
            sendEndPoint.Send(new WriteLogEvent()
            {
                Level = logLevel.ToString(),
                Logger = typeof(T).FullName,
                Message = message,
                Data = data == null ? "" : JsonConvert.SerializeObject(data)
            });
        }
    }
}
