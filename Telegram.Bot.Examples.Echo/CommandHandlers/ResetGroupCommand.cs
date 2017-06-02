using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Akavache;
using Bot.Services;
using Bot.Services.Commands;
using Telegram.Bot.Types;

namespace Telegram.Bot.Examples.Echo.CommandHandlers
{
    public class ResetGroupCommand : BaseCommand
    {
        private readonly DataService _dataService;
        public ResetGroupCommand(DataService dataService) : base(Command.ResetGroup)
        {
            _dataService = dataService;
        }

        public override async Task Handle(TelegramBotClient bot, Message message)
        {
            try
            {
                var data = message.Text.Split(' ');
                await _dataService.AddPost(data[1], data[2]);
                await bot.SendTextMessageAsync(message.Chat.Id, $"Ok");
            }
            catch (KeyNotFoundException)
            {
                // ignored
            }
        }
    }
}