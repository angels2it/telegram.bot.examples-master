using System.Threading.Tasks;
using Bot.Services;
using Bot.Services.Commands;
using Microsoft.Bot.Builder.Dialogs;

namespace Bot.Web.CommandHandlers
{
    public class UpdateGroupCommand : BaseCommand
    {
        public UpdateGroupCommand() : base(Command.UpdateGroup)
        {
        }

        public override async Task Handle(IDialogContext bot, string message)
        {
            await MainService.InitGroups();
            await bot.PostAsync($"Ok");
        }
    }
}