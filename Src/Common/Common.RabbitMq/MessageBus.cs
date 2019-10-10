using Common.General.Interfaces;
using Common.General.SharedKernel;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using RabbitMQ.Client.Exceptions;
using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Common.RabbitMq
{
    public class MessageBus : IMessageBus
    {
        private readonly IBusControl _busControl;
        private readonly IConfiguration _configuration;
        private readonly ILogger<MessageBus> _logger;
        public MessageBus(IConfiguration configuration, IBusControl busControl, ILogger<MessageBus> logger)
        {
            _busControl = busControl;
            _configuration = configuration;
            _logger = logger;
        }


        public Task Send<T>(
            string channel,
            T message,
            Type messageType,
            CancellationToken cancellationToken = default(CancellationToken)) where T : class
        {
            var policy = RetryPolicy.Handle<SocketException>()
                  .Or<BrokerUnreachableException>()
                  .WaitAndRetry(5, retryAttempt => TimeSpan.FromSeconds(
                      Math.Pow(2, retryAttempt)), (ex, time) =>
                      {
                          _logger.LogWarning(
                              ex,
                              "RabbitMQ Client could not Publish after {TimeOut}s ({ExceptionMessage})",
                              $"{time.TotalSeconds:n1}", ex.Message);
                      }
              );
            policy.Execute(() =>
            {
                var sendEndPoint = _busControl.GetSendEndpoint(
                new Uri($"{_configuration.GetConnectionString(ApplicationConstants.MessageBusHost)}/{channel}")).Result;
                sendEndPoint.Send(message, messageType, cancellationToken);
                return Task.CompletedTask;
            });
            return Task.FromException(new BrokerUnreachableException(new Exception()));
        }

        public async Task SendAsync<T>(
            string channel,
            T message,
            Type messageType,
            CancellationToken cancellationToken = default(CancellationToken)) where T : class
        {
            var policy = RetryPolicy.Handle<SocketException>()
                   .Or<BrokerUnreachableException>()
                   .WaitAndRetry(5, retryAttempt => TimeSpan.FromSeconds(
                       Math.Pow(2, retryAttempt)), (ex, time) =>
                       {
                           _logger.LogWarning(
                               ex,
                               "RabbitMQ Client could not Send after {TimeOut}s ({ExceptionMessage})",
                               $"{time.TotalSeconds:n1}", ex.Message);
                       }
               );
            await policy.Execute(async () =>
            {
                var sendEndPoint = _busControl.GetSendEndpoint(
                new Uri($"{_configuration.GetConnectionString(ApplicationConstants.MessageBusHost)}/{channel}")).Result;
                await sendEndPoint.Send(message, messageType, cancellationToken);
            });
        }

        public Task Send<T>(
            string channel,
            T message,
            CancellationToken cancellationToken = default(CancellationToken)) where T : class
        {
            var policy = RetryPolicy.Handle<SocketException>()
                 .Or<BrokerUnreachableException>()
                 .WaitAndRetry(5, retryAttempt => TimeSpan.FromSeconds(
                     Math.Pow(2, retryAttempt)), (ex, time) =>
                     {
                         _logger.LogWarning(
                             ex,
                             "RabbitMQ Client could not Publish after {TimeOut}s ({ExceptionMessage})",
                             $"{time.TotalSeconds:n1}", ex.Message);
                     }
             );
            policy.Execute(() =>
            {
                var sendEndPoint = _busControl.GetSendEndpoint(
                    new Uri($"{_configuration.GetConnectionString(ApplicationConstants.MessageBusHost)}/{channel}")).Result;
                sendEndPoint.Send(message, cancellationToken);

                return Task.CompletedTask;
            });
            return Task.FromException(new BrokerUnreachableException(new Exception()));
        }

        public async Task SendAsync<T>(
            string channel,
            T message,
            CancellationToken cancellationToken = default(CancellationToken)) where T : class
        {
            var policy = RetryPolicy.Handle<SocketException>()
                    .Or<BrokerUnreachableException>()
                    .WaitAndRetry(5, retryAttempt => TimeSpan.FromSeconds(
                        Math.Pow(2, retryAttempt)), (ex, time) =>
                        {
                            _logger.LogWarning(
                                ex,
                                "RabbitMQ Client could not Send after {TimeOut}s ({ExceptionMessage})",
                                $"{time.TotalSeconds:n1}", ex.Message);
                        }
                );
            await policy.Execute(async () =>
             {
                 var sendEndPoint = _busControl.GetSendEndpoint(
                   new Uri($"{_configuration.GetConnectionString(ApplicationConstants.MessageBusHost)}/{channel}")).Result;
                 await sendEndPoint.Send(message, cancellationToken);
             });

        }

        public Task Publish<T>(
            T message,
            Type messageType,
            CancellationToken cancellationToken = default(CancellationToken)) where T : class
        {

            var policy = RetryPolicy.Handle<SocketException>()
                    .Or<BrokerUnreachableException>()
                    .WaitAndRetry(5, retryAttempt => TimeSpan.FromSeconds(
                        Math.Pow(2, retryAttempt)), (ex, time) =>
                        {
                            _logger.LogWarning(
                                ex,
                                "RabbitMQ Client could not Publish after {TimeOut}s ({ExceptionMessage})",
                                $"{time.TotalSeconds:n1}", ex.Message);
                        }
                );
            policy.Execute(() =>
           {
               _busControl.Publish(message, messageType, cancellationToken);
               return Task.CompletedTask;
           });
            return Task.FromException(new BrokerUnreachableException(new Exception()));
        }

        public async Task PublishAsync<T>(
            T message,
            Type messageType,
            CancellationToken cancellationToken = default(CancellationToken)) where T : class
        {
            var policy = RetryPolicy.Handle<SocketException>()
                   .Or<BrokerUnreachableException>()
                   .WaitAndRetry(5, retryAttempt => TimeSpan.FromSeconds(
                       Math.Pow(2, retryAttempt)), (ex, time) =>
                       {
                           _logger.LogWarning(
                               ex,
                               "RabbitMQ Client could not Publish after {TimeOut}s ({ExceptionMessage})",
                               $"{time.TotalSeconds:n1}", ex.Message);
                       }
               );
            await policy.Execute(async () =>
            {
                await _busControl.Publish(message, messageType, cancellationToken);
            });

        }
    }
}
