using Common.General.Helper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Common.RabbitMq
{
    internal partial class EventBusRabbitMQ : IEventBus, IDisposable
    {
        private const string BROKER_NAME = "ClincApp";
        private const int DefaultRetryCount = 5;
        private readonly IRabbitMQPersistentConnection _persistentConnection;
        private readonly ILogger<EventBusRabbitMQ> _logger;
        private readonly int _retryCount;
        private readonly IServiceScopeFactory _factory;
        private IModel _consumerChannel;
        private string _queueName;

        public EventBusRabbitMQ(
            IRabbitMQPersistentConnection rabbitMQPersistentConnection,
            ILogger<EventBusRabbitMQ> logger,
            IServiceScopeFactory factory,//TODO: (Kmuhammad) replace with IServiceProvider
            IConfiguration configuration)
        {
            _persistentConnection = rabbitMQPersistentConnection ?? throw new ArgumentNullException(nameof(rabbitMQPersistentConnection));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _queueName = configuration["SubscriptionClientName"];
            _consumerChannel = CreateConsumerChannel();
            _retryCount = configuration.GetValue<int>("EventBusRetryCount", DefaultRetryCount);
            _factory = factory;
        }

        public void Publish(IntegrationEvent @event)
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }
            var policy = RetryPolicy.Handle<BrokerUnreachableException>()
                .Or<SocketException>()
                .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (exception, time) =>
                {
                    _logger.LogWarning(
                        exception,
                        "Could not publish event: {EventId} after {Timeout}s ({ExceptionMessage})",
                        @event.Id,
                        $"{time.TotalSeconds:n1}", exception.Message);
                });
            var eventName = @event.GetType().Name;
            _logger.LogTrace(
                "Creating RabbitMQ channel to publish event: {EventId} ({EventName})",
                @event.Id,
                eventName);

            using (var channel = _persistentConnection.CreateModel())
            {
                _logger.LogTrace("Declaring RabbitMQ exchange to publish event: {EventId}", @event.Id);
                channel.ExchangeDeclare(exchange: BROKER_NAME, type: "direct");
                var message = JsonHelper.Serialize(@event);
                var body = Encoding.UTF8.GetBytes(message);
                policy.Execute(() =>
                {
                    var properties = channel.CreateBasicProperties();
                    properties.DeliveryMode = 2; // persistent

                    _logger.LogTrace(
                        "Publishing event to RabbitMQ: {EventId}",
                        @event.Id);
                    channel.BasicPublish(BROKER_NAME, eventName, true, properties, body);
                });
            }
        }

        public void Dispose()
        {
            if (_consumerChannel != null)
            {
                _consumerChannel.Dispose();
            }
            ClearEventTypes();
        }

        private void StartBasicConsume()
        {
            _logger.LogTrace("Starting RabbitMQ basic consume");

            if (_consumerChannel != null)
            {
                var consumer = new AsyncEventingBasicConsumer(_consumerChannel);
                consumer.Received += Consumer_Received;
                _consumerChannel.BasicConsume(_queueName, false, consumer);
            }
            else
            {
                _logger.LogError("StartBasicConsume can't call on _consumerChannel == null");
            }
        }

        private async Task Consumer_Received(object sender, BasicDeliverEventArgs eventArgs)
        {
            var eventName = eventArgs.RoutingKey;
            var message = Encoding.UTF8.GetString(eventArgs.Body);
            try
            {
                if (message.ToLowerInvariant().Contains("throw-fake-exception"))
                {
                    throw new InvalidOperationException($"Fake exception requested: \"{message}\"");
                }

                await ProcessEvent(eventName, message);
            }
            catch (Exception exception)
            {
                _logger.LogWarning(exception, "----- ERROR Processing message \"{Message}\"", message);
            }
            _consumerChannel.BasicAck(eventArgs.DeliveryTag, multiple: false);
        }

        private RabbitMQ.Client.IModel CreateConsumerChannel()
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }
            _logger.LogTrace("Creating RabbitMQ consumer channel {queueName}", _queueName);
            var channel = _persistentConnection.CreateModel();
            channel.ExchangeDeclare(BROKER_NAME, "direct");
            channel.QueueDeclare(_queueName, true, false, false, null);
            channel.CallbackException += (sender, ea) =>
            {
                _logger.LogWarning(ea.Exception, "Recreating RabbitMQ consumer channel");
                _consumerChannel.Dispose();
                _consumerChannel = CreateConsumerChannel();
                StartBasicConsume();
            };
            return channel;
        }

        private async Task ProcessEvent(string eventName, string message)
        {
            _logger.LogTrace("Processing RabbitMQ event: {EventName}", eventName);
            if (this.TryGetEventTypeByName(eventName, out var eventType))
            {
                using (var scope = _factory.CreateScope())
                {
                    var handlerConcreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);
                    var eventHandlers = scope.ServiceProvider.GetServices(handlerConcreteType);
                    foreach (var eventHandler in eventHandlers)
                    {
                        var integrationEvent = JsonHelper.Deserialize(message, eventType);
                        await Task.Yield();
                        await (Task)handlerConcreteType.GetMethod("Handle").Invoke(eventHandler, new object[] { integrationEvent });
                    }
                }
            }
            else
            {
                _logger.LogWarning("No subscription for RabbitMQ event: {EventName}", eventName);
            }
        }
    }
}
