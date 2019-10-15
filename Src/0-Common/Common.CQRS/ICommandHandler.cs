using MediatR;

namespace Common.CQRS
{
    public interface ICommandHandler<TCommand> : INotificationHandler<TCommand>
        where TCommand : ICommand
    {

    }
}
