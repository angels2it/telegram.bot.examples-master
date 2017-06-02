using System.Threading.Tasks;
using Bot.Services.Commands;
using Telegram.Bot.Types;

namespace Telegram.Bot.Examples.Echo.CommandHandlers
{
    public abstract class BaseCommand : ICommandHandler
    {
        public Command Command { get; private set; }

        protected BaseCommand(Command command)
        {
            Command = command;
        }
        public abstract Task Handle(TelegramBotClient bot, Message message);
    }
}