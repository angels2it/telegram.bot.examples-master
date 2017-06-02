using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bot.Services.Commands;
using Telegram.Bot.Types;

namespace Telegram.Bot.Examples.Echo.CommandHandlers
{
    public class UpdateCommand : BaseCommand
    {
        private readonly Operation _operation;
        public UpdateCommand(Operation operation) : base(Command.Update)
        {
            _operation = operation;
        }

        public override async Task Handle(TelegramBotClient bot, Message message)
        {
            var groupId = message.Text.Split(' ')[1];
            await _operation.Update(groupId);
        }
    }
}
