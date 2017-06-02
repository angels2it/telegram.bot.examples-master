using System;
using System.Threading.Tasks;
using Bot.Services;
using Bot.Web.CommandHandlers;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace Bot.Web.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        static DataService DataService = new DataService();
        private static readonly CommandList _commandList = new CommandList()
        {
            new StartCommand(DataService),
            new ResetGroupCommand(DataService),
            new ResetCommand(),
            new EndCommand(DataService),
            new UpdateGroupCommand(),
            //new UpdateCommand(Operation)
        };
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            await _commandList.Process(context, activity?.Text);
            // return our reply to the user
            context.Wait(MessageReceivedAsync);
        }
    }
}