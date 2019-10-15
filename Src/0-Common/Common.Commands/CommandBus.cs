using Common.General.Exceptions;
using System;
using System.Threading.Tasks;

namespace Common.Commands
{
    public class CommandBus : ICommandBus
    {
        private readonly IServiceProvider _provider;


        public CommandBus(IServiceProvider provider)
        {
            _provider = provider;
        }

        public Task ExecuteAsync<TCommand>(TCommand command) where TCommand : ICommand
        {
            if (command == null)
                throw new ArgumentNullException("command");

            //if (!command.Validate(_validationContext))
            //    throw new ValidationErrorException(_validationContext.FormatValidationError());

            var handler = _provider.GetService(typeof(ICommandHandler<TCommand>));

            if (handler == null)
                throw new CommandHandlerNotFoundException();

            return (handler as ICommandHandler<TCommand>)?.HandleAsync(command);
        }
    }
}
