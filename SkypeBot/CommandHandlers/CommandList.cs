using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bot.Services;
using Microsoft.Bot.Builder.Dialogs;
using Topshelf.Logging;

namespace Bot.Web.CommandHandlers
{
    public class CommandList : List<ICommandHandler>
    {
        public Task Process(IDialogContext bot, string message)
        {
            if (!message.StartsWith("/"))
                return Task.FromResult(true);
            var command = message.Split(' ')[0].Trim('/').ToLower();
            var processer = this.FirstOrDefault(e => e.Command.ToString().ToLower() == command);
            try
            {
                return processer?.Handle(bot, message);
            }
            catch (Exception e)
            {
                MainService.Logger.Log(LoggingLevel.Error, e);
                return bot.PostAsync(e.StackTrace);
            }
        }
    }
}
