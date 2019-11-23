using System;

namespace Common.RabbitMq
{
    internal interface IEventBusSubscriptionsManager
    {
        void AddSubscription(Type eventType);
    }
}
