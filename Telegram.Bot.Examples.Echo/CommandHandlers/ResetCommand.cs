using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Akavache;
using Bot.Services;
using Bot.Services.Commands;
using Telegram.Bot.Types;

namespace Telegram.Bot.Examples.Echo.CommandHandlers
{
    public class ResetCommand : BaseCommand
    {
        public ResetCommand() : base(Command.Reset)
        {
        }

        public override async Task Handle(TelegramBotClient bot, Message message)
        {
            try
            {
                await BlobCache.LocalMachine.InvalidateObject<List<GroupModel>>("ChatIds");
                await bot.SendTextMessageAsync(message.Chat.Id, $"Ok");
            }
            catch (KeyNotFoundException)
            {
                // ignored
            }
        }
    }
}