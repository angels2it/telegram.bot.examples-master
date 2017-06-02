using System.Threading.Tasks;
using Bot.Services;
using Bot.Services.Commands;
using Microsoft.Bot.Builder.Dialogs;

namespace Bot.Web.CommandHandlers
{
    public class EndCommand : BaseCommand
    {
        private readonly DataService _dataService;
        public EndCommand(DataService dataService) : base(Command.End)
        {
            _dataService = dataService;
        }

        public override async Task Handle(IDialogContext bot, string message)
        {
            await _dataService.RemoveChatId(bot.Activity.Id);
            await bot.PostAsync("Ok, I'm removed");
        }

    }
}