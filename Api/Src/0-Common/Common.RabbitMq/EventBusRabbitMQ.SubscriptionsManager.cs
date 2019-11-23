using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;

namespace Common.RabbitMq
{
    internal partial class EventBusRabbitMQ : IEventBusSubscriptionsManager
    {
        private readonly Dictionary<string, Type> _eventTypes = new Dictionary<string, Type>();

        public void AddSubscription(Type eventType)
        {
            var eventName = GetEventKey(eventType);
            InternalSubscription(eventName);
            _logger.LogInformation("Subscribing to event {EventName}", eventName);
            StartBasicConsume();
            if (!_eventTypes.ContainsKey(eventName))
            {
                _eventTypes.Add(eventName, eventType);
            }
        }

        private void InternalSubscription(string eventName)
        {
            var containsKey = _eventTypes.ContainsKey(eventName);
            if (!containsKey)
            {
                if (!_persistentConnection.IsConnected)
                {
                    _persistentConnection.TryConnect();
                }
                using (var channel = _persistentConnection.CreateModel())
                {
                    channel.QueueBind(_queueName, BROKER_NAME, eventName);
                }
            }
        }

        private void ClearEventTypes() => _eventTypes.Clear();

        private bool TryGetEventTypeByName(string eventName, out Type eventType)
        {
            return _eventTypes.TryGetValue(eventName, out eventType);
        }

        private string GetEventKey(Type eventType)
        {
            return eventType.Name;
        }
    }
}
