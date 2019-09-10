using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common.Events
{
    public class EventBus : IEventBus
    {
        private readonly IServiceProvider _provider;

        public EventBus(IServiceProvider provider)
        {
            _provider = provider;
        }

        public Task ExecuteAsync<TEvent>(TEvent @event) where TEvent : IEvent
        {
            if (@event == null)
                throw new ArgumentNullException("event");

            Type elementType = @event.GetType();
            Type repositoryType = typeof(IEventHandler<>).MakeGenericType(elementType);
            var handler = _provider.GetService(repositoryType);

            if (handler != null)
            {
                MethodInfo method = repositoryType.GetMethod("ExecuteAsync");
                MethodInfo genericMethod = method.MakeGenericMethod(elementType);

                return (Task)genericMethod.Invoke(handler, new[] { Convert.ChangeType(@event, elementType) });
            }

            return Task.CompletedTask;
        }
    }
}
