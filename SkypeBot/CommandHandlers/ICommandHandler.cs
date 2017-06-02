using System.Threading.Tasks;
using Bot.Services.Commands;
using Microsoft.Bot.Builder.Dialogs;

namespace Bot.Web.CommandHandlers
{
    public interface ICommandHandler
    {
        Command Command { get; }
        Task Handle(IDialogContext bot, string message);
    }
}