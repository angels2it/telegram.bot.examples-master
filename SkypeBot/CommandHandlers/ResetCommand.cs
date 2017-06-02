using System.Collections.Generic;
using System.Threading.Tasks;
using Bot.Services;
using Bot.Services.Commands;
using Microsoft.Bot.Builder.Dialogs;

namespace Bot.Web.CommandHandlers
{
    public class ResetCommand : BaseCommand
    {
        public ResetCommand() : base(Command.Reset)
        {
        }

        public override async Task Handle(IDialogContext bot, string message)
        {
            try
            {
                await BlobCache.LocalMachine.InvalidateObject<List<GroupModel>>("ChatIds");
                await bot.PostAsync($"Ok");
            }
            catch (KeyNotFoundException)
            {
                // ignored
            }
        }
    }
}