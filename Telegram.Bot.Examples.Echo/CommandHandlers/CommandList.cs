using System;
using System.Collections.Generic;
using System.Linq;
using Bot.Services;
using Telegram.Bot.Types;
using Topshelf.Logging;

namespace Telegram.Bot.Examples.Echo.CommandHandlers
{
    public class CommandList : List<ICommandHandler>
    {
        public void Process(TelegramBotClient bot, Message message)
        {
            if (!message.Text.StartsWith("/"))
                return;
            var command = message.Text.Split(' ')[0].Trim('/').ToLower();
            var processer = this.FirstOrDefault(e => e.Command.ToString().ToLower() == command);
            try
            {
                processer?.Handle(bot, message);
            }
            catch (Exception e)
            {
                MainService.Logger.Log(LoggingLevel.Error, e);
                bot.SendTextMessageAsync(message.Chat.Id, e.StackTrace);
            }
        }
    }
}
