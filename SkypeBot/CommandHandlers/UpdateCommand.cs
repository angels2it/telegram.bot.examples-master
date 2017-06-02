using System.Threading.Tasks;
using Bot.Services;
using Bot.Services.Commands;
using Microsoft.Bot.Builder.Dialogs;

namespace Bot.Web.CommandHandlers
{
    public class UpdateCommand : BaseCommand
    {
        private readonly MainService _operation;
        public UpdateCommand(MainService operation) : base(Command.Update)
        {
            _operation = operation;
        }

        public override async Task Handle(IDialogContext bot, string message)
        {
            var groupId = message.Split(' ')[1];
            //await _operation.(groupId);
        }
    }
}
