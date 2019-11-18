using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Common.RabbitMq;

namespace Common.RabbitMq
{
    public interface IIntegrationEventHandler<in TIntegrationEvent>
        where TIntegrationEvent : IntegrationEvent
    {
        Task Handle(TIntegrationEvent @event);
    }
}
