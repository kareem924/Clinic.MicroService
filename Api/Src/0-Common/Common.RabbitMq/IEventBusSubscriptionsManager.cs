using System;
using System.Collections.Generic;
using System.Text;
using Common.RabbitMq;

namespace Common.RabbitMq
{
    internal interface IEventBusSubscriptionsManager
    {
        void AddSubscription(Type eventType);
    }
}
