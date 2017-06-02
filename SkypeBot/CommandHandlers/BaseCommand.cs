using System.Threading.Tasks;
using Bot.Services.Commands;
using Microsoft.Bot.Builder.Dialogs;

namespace Bot.Web.CommandHandlers
{
    public abstract class BaseCommand : ICommandHandler
    {
        public Command Command { get; private set; }

        protected BaseCommand(Command command)
        {
            Command = command;
        }
        public abstract Task Handle(IDialogContext bot, string message);
    }
}