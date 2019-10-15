using System.Threading.Tasks;

namespace Common.Events
{
    public interface IEventBus
    {
        Task ExecuteAsync<TEvent>(TEvent @event) where TEvent : IEvent;
    }
}
