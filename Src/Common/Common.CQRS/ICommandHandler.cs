using System;
using System.Collections.Generic;
using System.Text;

namespace Common.CQRS
{
    public interface ICommandHandler<TCommand> : INotificationHandler<TCommand>
        where TCommand : ICommand
    {
    }
}
