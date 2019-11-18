using System;
using System.Collections.Generic;
using System.Text;

namespace Common.RabbitMq
{
    public interface IEventBus
    {
        void Publish(IntegrationEvent @event);
    }
}
