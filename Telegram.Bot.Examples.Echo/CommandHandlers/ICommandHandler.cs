using System.Threading.Tasks;
using Bot.Services.Commands;
using Telegram.Bot.Types;

namespace Telegram.Bot.Examples.Echo.CommandHandlers
{
    public interface ICommandHandler
    {
        Command Command { get; }
        Task Handle(TelegramBotClient bot, Message message);
    }
}