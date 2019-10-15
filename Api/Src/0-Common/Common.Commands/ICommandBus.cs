using System.Threading.Tasks;

namespace Common.Commands
{
    public interface ICommandBus
    {
        Task ExecuteAsync<TCommand>(TCommand command) where TCommand : ICommand;
    }
}
