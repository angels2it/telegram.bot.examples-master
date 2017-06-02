using System.Collections.Generic;
using System.Threading.Tasks;
using Bot.Services;
using Bot.Services.Commands;
using Microsoft.Bot.Builder.Dialogs;

namespace Bot.Web.CommandHandlers
{
    public class ResetGroupCommand : BaseCommand
    {
        private readonly DataService _dataService;
        public ResetGroupCommand(DataService dataService) : base(Command.ResetGroup)
        {
            _dataService = dataService;
        }

        public override async Task Handle(IDialogContext bot, string message)
        {
            try
            {
                var data = message.Split(' ');
                await _dataService.AddPost(data[1], data[2]);
                await bot.PostAsync($"Ok");
            }
            catch (KeyNotFoundException)
            {
                // ignored
            }
        }
    }
}