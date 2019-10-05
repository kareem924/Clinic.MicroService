using Common.Events;
using Common.General.SharedKernel;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;

namespace Common.RabbitMq
{
    public abstract class BaseConsumer
    {
        protected void WriteErrorLog<T>(ConsumeContext<T> context, IConfiguration configuration, string message, object data) where T : class
        {
            WriteLog(context, configuration, LogLevel.Error, message, data);
        }

        protected  void WriteInformationLog<T>(ConsumeContext<T> context, IConfiguration configuration, string message) where T : class
        {
            WriteLog(context, configuration, LogLevel.Information, message);
        }

        private void WriteLog<T>(
            ConsumeContext<T> context,
            IConfiguration configuration,
            LogLevel logLevel,
            string message,
            object data = null) where T : class
        {
            var sendEndPoint = context.GetSendEndpoint(
                new Uri($"{configuration.GetConnectionString(ApplicationConstants.MessageBusHost)}/" +
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
