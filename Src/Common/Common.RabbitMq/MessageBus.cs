using Common.General.Interfaces;
using Common.General.SharedKernel;
using MassTransit;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common.RabbitMq
{
    public class MessageBus : IMessageBus
    {
        private readonly IBusControl _busControl;
        private readonly IConfiguration _configuration;

        public MessageBus(IConfiguration configuration, IBusControl busControl)
        {
            _busControl = busControl;
            _configuration = configuration;
        }

        public Task Send<T>(
            string channel, 
            T message, 
            Type messageType, 
            CancellationToken cancellationToken = default(CancellationToken)) where T : class
        {
            var sendEndPoint = _busControl.GetSendEndpoint(
                new Uri($"{_configuration.GetConnectionString(ApplicationConstants.MessageBusHost)}/{channel}")).Result;
            sendEndPoint.Send(message, messageType);

            return Task.CompletedTask;
        }

        public async Task SendAsync<T>(
            string channel, 
            T message, 
            Type messageType, 
            CancellationToken cancellationToken = default(CancellationToken)) where T : class
        {
            var sendEndPoint = _busControl.GetSendEndpoint(
                new Uri($"{_configuration.GetConnectionString(ApplicationConstants.MessageBusHost)}/{channel}")).Result;
            await sendEndPoint.Send(message, messageType);
        }

        public Task Send<T>(
            string channel, 
            T message, 
            CancellationToken cancellationToken = default(CancellationToken)) where T : class
        {
            var sendEndPoint = _busControl.GetSendEndpoint(
                new Uri($"{_configuration.GetConnectionString(ApplicationConstants.MessageBusHost)}/{channel}")).Result;
            sendEndPoint.Send(message);

            return Task.CompletedTask;
        }

        public async Task SendAsync<T>(
            string channel, 
            T message, 
            CancellationToken cancellationToken = default(CancellationToken)) where T : class
        {
            var sendEndPoint = _busControl.GetSendEndpoint(
                new Uri($"{_configuration.GetConnectionString(ApplicationConstants.MessageBusHost)}/{channel}")).Result;
            await sendEndPoint.Send(message);
        }

        public Task Publish<T>(
            T message, 
            Type messageType, 
            CancellationToken cancellationToken = default(CancellationToken)) where T : class
        {
            _busControl.Publish(message, messageType);
            return Task.CompletedTask;
        }

        public async Task PublishAsync<T>(
            T message, 
            Type messageType, 
            CancellationToken cancellationToken = default(CancellationToken)) where T : class
        {
            await _busControl.Publish(message, messageType);
        }
    }
}
