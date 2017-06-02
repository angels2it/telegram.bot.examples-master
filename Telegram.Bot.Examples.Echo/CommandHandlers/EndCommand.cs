using System.Threading.Tasks;
using Bot.Services;
using Bot.Services.Commands;
using Telegram.Bot.Types;

namespace Telegram.Bot.Examples.Echo.CommandHandlers
{
    public class EndCommand : BaseCommand
    {
        private readonly DataService _dataService;
        public EndCommand(DataService dataService) : base(Command.End)
        {
            _dataService = dataService;
        }

        public override async Task Handle(TelegramBotClient bot, Message message)
        {
            await _dataService.RemoveChatId(message.Chat.Id);
            await bot.SendTextMessageAsync(message.Chat.Id, "Ok, I'm removed");
        }

    }
}