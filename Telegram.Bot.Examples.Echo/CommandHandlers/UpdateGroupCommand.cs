using System;
using System.Threading.Tasks;
using Bot.Services.Commands;
using Telegram.Bot.Types;

namespace Telegram.Bot.Examples.Echo.CommandHandlers
{
    public class UpdateGroupCommand : BaseCommand
    {
        public UpdateGroupCommand() : base(Command.UpdateGroup)
        {
        }

        public override async Task Handle(TelegramBotClient bot, Message message)
        {
            await TelegramService.InitGroups();
            await bot.SendTextMessageAsync(message.Chat.Id, $"Ok");
        }
    }
}