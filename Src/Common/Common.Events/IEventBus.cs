using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Common.Events
{
    public interface IEventBus
    {
        Task ExecuteAsync<TEvent>(TEvent @event) where TEvent : IEvent;
    }
}
