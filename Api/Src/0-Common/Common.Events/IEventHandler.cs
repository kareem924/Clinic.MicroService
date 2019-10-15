using System.Threading.Tasks;

namespace Common.Events
{
    public interface IEventHandler<in T> where T : IEvent
    {
        Task HandleAsync(T command);
    }
}
