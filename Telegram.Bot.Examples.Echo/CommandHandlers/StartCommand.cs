using System.Threading.Tasks;
using Bot.Services;
using Bot.Services.Commands;
using Telegram.Bot.Types;

namespace Telegram.Bot.Examples.Echo.CommandHandlers
{
    public class StartCommand : BaseCommand
    {
        private readonly DataService _dataService;
        public StartCommand(DataService dataService) : base(Command.Start)
        {
            _dataService = dataService;
        }

        public override async Task Handle(TelegramBotClient bot, Message message)
        {
            await _dataService.AddChatId(message.Chat.Id);
            await bot.SendTextMessageAsync(message.Chat.Id, "OK");
            //var groups = TelegramService.Groups.Select(e => new InlineKeyboardButton(e.Id, e.Id)).ToArray();
            //var keyboard = new InlineKeyboardMarkup(new[]
            //{
            //    groups
            //});
            //await bot.SendTextMessageAsync(message.Chat.Id, "Choose group you want to receive update",
            //    replyMarkup: keyboard);
        }
    }
}